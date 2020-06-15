using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodyPlumber
{
    public class PowerupSystem
    {
        private ScreenManager m_manager;
        public Weapon ak47;
        public Weapon laser;

        public void Initialize(ScreenManager manager)
        {
            m_manager = manager;

            ak47 = new Weapon();
            laser = new Weapon();
        }

        public void LoadContent()
        {
            ak47.Initialize(-1000, -1000, m_manager.imageFileSystem.akAnimation, m_manager.imageFileSystem.akProjectile, 50, m_manager.audioFileSystem.akShot, 60, m_manager);
            laser.Initialize(-1000, -1000, m_manager.imageFileSystem.laserAnimation, m_manager.imageFileSystem.laserProjectile, 300, m_manager.audioFileSystem.laserShot, 2000, m_manager);
        }

        
    }
}
