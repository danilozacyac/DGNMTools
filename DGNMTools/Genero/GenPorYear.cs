using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGNMTools.Genero
{
    public class GenPorYear
    {
        private int year;
        private int hombres;
        private int mujeres;
        private int total;

        public int Year
        {
            get
            {
                return this.year;
            }
            set
            {
                this.year = value;
            }
        }

        public int Hombres
        {
            get
            {
                return this.hombres;
            }
            set
            {
                this.hombres = value;
            }
        }

        public int Mujeres
        {
            get
            {
                return this.mujeres;
            }
            set
            {
                this.mujeres = value;
            }
        }

        public int Total
        {
            get
            {
                return this.total;
            }
            set
            {
                this.total = value;
            }
        }
    }
}
