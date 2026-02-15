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

        public async Task<Present> MakeLottery(int presentId)
        {
            var present = await _context.Presents.FirstOrDefaultAsync(p => p.Id == presentId);

            if (present == null || (present.WinnerId != null && present.WinnerId != 0))
                return null;

            var relevantPurchases = await _context.Purchases
                .Where(p => p.PresentId == presentId)
                .Include(p => p.Person)
                .ToListAsync();

            if (relevantPurchases == null || !relevantPurchases.Any())
                return null;

            List<Person> peopleOfLottery = relevantPurchases
                .Where(p => p.Person != null)
                .Select(p => p.Person!)
                .ToList();

            var random = new Random();
            var winner = peopleOfLottery[random.Next(peopleOfLottery.Count)];

            present.WinnerId = winner.Id;
            present.Winner = winner;

            await SendWinnerEmail(winner, present.Name);
            await LogWinnerToFile(winner, present.Name);

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
                    mail.Body = $"<div dir='rtl'><h2>שלום {winner.FirstName}!</h2><p>זכית ב: <strong>{presentName}</strong></p></div>";
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
                string errorPath = "LotteryFiles/errors.txt";
                if (!Directory.Exists("LotteryFiles")) Directory.CreateDirectory("LotteryFiles");
                await File.AppendAllTextAsync(errorPath, $"{DateTime.Now}: Failed to email {winner.Email}. {ex.Message}{Environment.NewLine}");
            }
        }

        private async Task LogWinnerToFile(Person winner, string presentName)
        {
            string folderPath = "LotteryFiles";
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            string fileWinnersPath = Path.Combine(folderPath, "lottery_winners.txt");
            string content = $"Date: {DateTime.Now} | Present: {presentName} | Winner: {winner.FirstName} {winner.LastName}{Environment.NewLine}";
            await File.AppendAllTextAsync(fileWinnersPath, content);
        }
    }
}