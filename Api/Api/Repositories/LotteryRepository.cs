using Api.Data;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace Api.Repositories
{
    public class LotteryRepository : ILotteryRepository
    {
        private readonly LotteryDbContext _context;

        public LotteryRepository(LotteryDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// מבצע הגרלה עבור מתנה ספציפית מתוך רשימת הרוכשים ששילמו (Purchases)
        /// </summary>
        public async Task<Present> MakeLottery(int presentId)
        {
            // 1. שליפת המתנה כולל פרטי הזוכה אם כבר קיים (כדי למנוע הגרלה כפולה)
            var present = await _context.Presents
                .FirstOrDefaultAsync(p => p.Id == presentId);

            if (present == null)
                throw new Exception($"מתנה עם ID {presentId} לא נמצאה.");

            if (present.WinnerId != null && present.WinnerId != 0)
                throw new Exception("כבר בוצעה הגרלה למתנה זו, לא ניתן להגריל שוב.");

            // 2. שליפת כל הרכישות שבוצעו עבור המתנה הזו בלבד
            // אנחנו רצים על Purchases כי אלו רכישות סופיות ולא סלים זמניים
            var relevantPurchases = await _context.Purchases
                .Where(p => p.PresentId == presentId)
                .Include(p => p.Person)
                .ToListAsync();

            if (relevantPurchases == null || !relevantPurchases.Any())
            {
                throw new Exception("לא נמצאו רוכשים למתנה זו, לא ניתן לבצע הגרלה.");
            }

            // 3. בניית רשימת המשתתפים - כל שורת רכישה היא כרטיס הגרלה אחד
            List<Person> peopleOfLottery = relevantPurchases
                .Where(p => p.Person != null)
                .Select(p => p.Person!)
                .ToList();

            // 4. ביצוע ההגרלה באופן רנדומלי
            var random = new Random();
            int randomIndex = random.Next(peopleOfLottery.Count);
            var winner = peopleOfLottery[randomIndex];

            // 5. עדכון הזוכה בבסיס הנתונים
            present.WinnerId = winner.Id;
            present.Winner = winner;

            // 6. שליחת מייל לזוכה המאושר
            await SendWinnerEmail(winner, present.Name);

            // 7. רישום הזכייה בקובץ טקסט למעקב הנהלה
            await LogWinnerToFile(winner, present.Name);

            // 8. שמירת השינויים ב-DB (עדכון ה-WinnerId במתנה)
            await _context.SaveChangesAsync();
            return present;
        }

        private async Task SendWinnerEmail(Person winner, string presentName)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("R0527167315@gmail.com");
                    mail.To.Add(winner.Email);
                    mail.Subject = "מזל טוב! זכית בהגרלה";
                    mail.Body = $@"
                        <div dir='rtl'>
                            <h2>שלום {winner.FirstName}!</h2>
                            <p>שמחים לעדכן אותך שזכית במתנה: <strong>{presentName}</strong> בהגרלה שערכנו.</p>
                            <p>ניצור איתך קשר בהקדם לתיאום קבלת הפרס.</p>
                        </div>";
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("R0527167315@gmail.com", "hhli hiof qxwm spxi");
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                // רישום שגיאה אם המייל לא נשלח
                string errorPath = "LotteryFiles/errors.txt";
                await File.AppendAllTextAsync(errorPath, $"{DateTime.Now}: Failed to email {winner.Email} for {presentName}. Error: {ex.Message}{Environment.NewLine}");
            }
        }

        private async Task LogWinnerToFile(Person winner, string presentName)
        {
            string folderPath = "LotteryFiles";
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string fileWinnersPath = Path.Combine(folderPath, "lottery_winners.txt");
            string content = $"Date: {DateTime.Now:dd/MM/yyyy HH:mm} | Present: {presentName} | Winner: {winner.FirstName} {winner.LastName} | Email: {winner.Email} | Phone: {winner.Phone}{Environment.NewLine}";

            await File.AppendAllTextAsync(fileWinnersPath, content);
        }
    }
}