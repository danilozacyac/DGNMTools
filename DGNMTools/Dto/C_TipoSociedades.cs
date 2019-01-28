using System;
using System.Linq;

namespace DGNMTools.Dto
{
    public class C_TipoSociedades
    {
        private int id;
        private string siglas;
        private string siglasStr;
        private string tipoSociedad;
        private string tipoSociedadStr;
        private string subtipo;
        private string subtipoStr;
        private string subtipoStrWoSpaces;

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public string Siglas
        {
            get
            {
                return this.siglas;
            }
            set
            {
                this.siglas = value;
            }
        }

        public string SiglasStr
        {
            get
            {
                return this.siglasStr;
            }
            set
            {
                this.siglasStr = value;
            }
        }

        public string TipoSociedad
        {
            get
            {
                return this.tipoSociedad;
            }
            set
            {
                this.tipoSociedad = value;
            }
        }

        public string TipoSociedadStr
        {
            get
            {
                return this.tipoSociedadStr;
            }
            set
            {
                this.tipoSociedadStr = value;
            }
        }

        public string Subtipo
        {
            get
            {
                return this.subtipo;
            }
            set
            {
                this.subtipo = value;
            }
        }

        public string SubtipoStr
        {
            get
            {
                return this.subtipoStr;
            }
            set
            {
                this.subtipoStr = value;
            }
        }

        public string SubtipoStrWoSpaces
        {
            get
            {
                return this.subtipoStrWoSpaces;
            }
            set
            {
                this.subtipoStrWoSpaces = value;
            }
        }
    }
}
