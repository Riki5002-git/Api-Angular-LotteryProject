using Api.DTOs;
using Api.Interfaces;
using Api.Models;

namespace Api.Services
{
    public class DonorService : IDonorService
    {
        private IDonorRepository _donorRepository;
        public DonorService(IDonorRepository donorRepository)
        {
            _donorRepository = donorRepository;
        }

        public async Task<IEnumerable<DonorDTOs>> GetAllDonors()
        {
            var people = await _donorRepository.GetAllDonors();
            return people.Select(MapToResponseDto);
        }

        private static DonorDTOs MapToResponseDto(Donor donor)
        {
            return new DonorDTOs
            {
                Id = donor.Id,
                FirstName = donor.FirstName,
                LastName = donor.LastName,
                Password = donor.Password,
                UserName = donor.UserName,
                Email = donor.Email,
                Phone = donor.Phone
            };
        }

        public async Task AddDonor(DonorDTOs donorDto)
        {
            var donor = new Donor
            {
                FirstName = donorDto.FirstName,
                LastName = donorDto.LastName,
                Password = donorDto.Password,
                UserName = donorDto.UserName,
                Email = donorDto.Email,
                Phone = donorDto.Phone
            };
            await _donorRepository.AddDonor(donor);
        }

        public async Task<DonorDTOs?> GetDonorById(int id)
        {
            var donor = await _donorRepository.GetDonorById(id);
            if (donor == null) return null;
            return MapToResponseDto(donor);
        }

        public async Task UpdateDonor(int id, DonorDTOs DonorDto)
        {
            var existingDonor = await _donorRepository.GetDonorById(id);
            if (existingDonor == null) return;

            existingDonor.FirstName = DonorDto.FirstName;
            existingDonor.LastName = DonorDto.LastName;
            existingDonor.Email = DonorDto.Email;
            existingDonor.Phone = DonorDto.Phone;
            existingDonor.UserName = DonorDto.UserName;
            existingDonor.Password = DonorDto.Password;

            await _donorRepository.UpdateDonor(existingDonor);
        }

        public async Task DeleteDonor(int id)
        {
            await _donorRepository.DeleteDonor(id);
        }

        public async Task<DonorDTOs?> GetDonorByName(string firstName, string lastName)
        {
            var donor = await _donorRepository.GetDonorByName(firstName, lastName);
            if (donor == null) return null;
            return MapToResponseDto(donor);
        }

        public async Task<DonorDTOs?> GetDonorByEmail(string email)
        {
            var donor = await _donorRepository.GetDonorByEmail(email);
            if (donor == null) return null;
            return MapToResponseDto(donor);
        }

        public async Task<DonorDTOs?> GetDonorByPresent(string presentName)
        {
            var donor = await _donorRepository.GetDonorByPresent(presentName);
            if (donor == null) return null;
            return MapToResponseDto(donor);
        }

        public async Task<List<PresentDTOs?>> GetDonorsPresents(int id)
        {
            var presents = await _donorRepository.GetDonorsPresents(id);

            var presentsDto = presents.Select(p => new PresentDTOs
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                PictureUrl = p.PictureUrl,
                PurchasesAmount = p.PurchasesAmount,
                CategoryId = p.CategoryId,
                DonorId = p.DonorId
            }).ToList();
            return presentsDto;
        }
    }
}
