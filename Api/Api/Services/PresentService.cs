using Api.DTOs;
using Api.Interfaces;
using Api.Models;

namespace Api.Services
{
    public class PresentService : IPresentService
    {
        private IPresentRepository _presentRepository;
        private IDonorRepository _donorRepository;
        public PresentService(IPresentRepository presentRepository, IDonorRepository donorRepository)
        {
            _presentRepository = presentRepository;
            _donorRepository = donorRepository;
        }
        public async Task AddPictureUrl(int id, string url)
        {
            await _presentRepository.AddPictureUrl(id, url);
        }

        public async Task AddPresent(PresentDTOs presentDTO)
        {
            await _presentRepository.AddPresent(new Present
            {
                Name = presentDTO.Name,
                Description = presentDTO.Description,
                Price = presentDTO.Price,
                PurchasesAmount = presentDTO.PurchasesAmount,
                DonorId = presentDTO.DonorId,
                CategoryId = presentDTO.CategoryId,
                PictureUrl = presentDTO.PictureUrl
            });
        }

        public async Task DeletePresent(int id)
        {
            var existingPresent = await _presentRepository.GetPresentById(id);
            if (existingPresent == null)
            {
                throw new Exception("Present not found");
            }
            await _presentRepository.DeletePresent(id);
        }

        public async Task<List<PresentDTOs>> GetAllPresents()
        {
            var presents = await _presentRepository.GetAllPresents();
            return presents.Select(MapToResponseDto).ToList();
        }

        public async Task<DonorDTOs?> GetDonorsPresent(string PresentName)
        {
            var present = await _presentRepository.GetPresentsByPresentName(PresentName);
            if (present == null) return null;
            var donor = present.DonorId;
            if (donor == null) return null;
            var curDonor = await _donorRepository.GetDonorById(donor);
            if (curDonor == null) return null;
            return new DonorDTOs
            {
                Id = curDonor.Id,
                FirstName = curDonor.FirstName,
                LastName = curDonor.LastName,
                Email = curDonor.Email,
                Phone = curDonor.Phone
            };
        }

        public async Task<PresentDTOs?> GetPresentById(int id)
        {
            var present = await _presentRepository.GetPresentById(id);
            if (present == null) return null;
            return MapToResponseDto(present);
        }

        public async Task<double> GetPresentPrice(int id)
        {
            var present = await _presentRepository.GetPresentById(id);
            if (present == null)
            {
                throw new Exception("Present not found");
            }
            return present.Price;
        }

        public async Task<IEnumerable<PresentDTOs>> GetPresentsByDonorName(string donorName)
        {
            var presents = await _presentRepository.GetPresentsByDonorName(donorName);
            return presents.Select(MapToResponseDto);
        }

        public async Task<PresentDTOs?> GetPresentsByPresentName(string name)
        {
            var present = await _presentRepository.GetPresentsByPresentName(name);
            if (present == null) return null;
            return MapToResponseDto(present);
        }

        public async Task<IEnumerable<PresentDTOs>> GetPresentsByPurchasesAmount(int amount)
        {
            var presents = await _presentRepository.GetPresentsByPurchasesAmount(amount);
            return presents.Select(MapToResponseDto);
        }

        public async Task UpdatePresent(int id, PresentDTOs presentDTO)
        {
            var existingPresent = await _presentRepository.GetPresentById(id);
            if (existingPresent == null)
            {
                throw new Exception("Present not found");
            }
            existingPresent.Name = presentDTO.Name;
            existingPresent.Description = presentDTO.Description;
            existingPresent.Price = presentDTO.Price;
            existingPresent.PurchasesAmount = presentDTO.PurchasesAmount;
            existingPresent.DonorId = presentDTO.DonorId;
            existingPresent.CategoryId = presentDTO.CategoryId;
            existingPresent.PictureUrl = presentDTO.PictureUrl;

            await _presentRepository.UpdatePresent(existingPresent);
        }

        private static PresentDTOs MapToResponseDto(Present present)
        {
            return new PresentDTOs
            {
                Id = present.Id,
                Name = present.Name,
                Description = present.Description,
                DonorId = present.DonorId,
                Price = present.Price,
                PictureUrl = present.PictureUrl,
                CategoryId = present.CategoryId,
                PurchasesAmount = present.PurchasesAmount,
                WinnerId = present.WinnerId,
                Winner = present.Winner
            };
        }
    }
}
