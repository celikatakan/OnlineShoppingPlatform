using OnlineShoppingPlatform.Data.Entities;
using OnlineShoppingPlatform.Data.Repository;
using OnlineShoppingPlatform.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Business.Operations.Setting
{
    // SettingManager class implements the ISettingService interface and manages application settings
    public class SettingManager : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SettingEntity> _settingRepository;
        // Constructor to inject dependencies
        public SettingManager(IRepository<SettingEntity> settingRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _settingRepository = settingRepository;
        }
        // Retrieves the current maintenance state of the application
        public bool GetMaintenanceState()
        {
            var maintenanceState = _settingRepository.GetById(1).MaintenenceMode;

            return maintenanceState;
        }
        // Toggles the maintenance mode state and saves changes to the database
        public async Task ToggleMaintenence()
        {
            // Get the setting entity with ID 1
            var setting = _settingRepository.GetById(1);
            // Toggle the maintenance mode state
            setting.MaintenenceMode = !setting.MaintenenceMode;
            // Update the setting in the repository
            _settingRepository.Update(setting);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Bakım durumunda hata oluştu.");
            }
        }
    }
}
