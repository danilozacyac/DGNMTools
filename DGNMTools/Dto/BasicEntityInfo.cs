using System;
using System.Linq;

namespace DGNMTools.Dto
{
    public class BasicEntityInfo
    {
        private int id;
        private int idGiro;
        private string giro;
        private string sociedad;
        private string sociedadStr;
        private string afterComa;
        private string afterComaStr;
        private string tipoSociedad;
        private bool modificado;

        private string denomDatoCadena;
        private string boletaInscripcion;
        private bool isSameName = false;


        public bool IsSameName
        {
            get
            {
                return this.isSameName;
            }
            set
            {
                this.isSameName = value;
            }
        }

        public string DenomDatoCadena
        {
            get
            {
                return this.denomDatoCadena;
            }
            set
            {
                this.denomDatoCadena = value;
            }
        }

        public string BoletaInscripcion
        {
            get
            {
                return this.boletaInscripcion;
            }
            set
            {
                this.boletaInscripcion = value;
            }
        }

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

        public int IdGiro
        {
            get
            {
                return this.idGiro;
            }
            set
            {
                this.idGiro = value;
            }
        }

        public string Giro
        {
            get
            {
                return this.giro;
            }
            set
            {
                this.giro = value;
            }
        }

        public string Sociedad
        {
            get
            {
                return this.sociedad;
            }
            set
            {
                this.sociedad = value;
            }
        }

        public string SociedadStr
        {
            get
            {
                return this.sociedadStr;
            }
            set
            {
                this.sociedadStr = value;
            }
        }

        public string AfterComa
        {
            get
            {
                return this.afterComa;
            }
            set
            {
                this.afterComa = value;
            }
        }

        public string AfterComaStr
        {
            get
            {
                return this.afterComaStr;
            }
            set
            {
                this.afterComaStr = value;
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

        public bool Modificado
        {
            get
            {
                return this.modificado;
            }
            set
            {
                this.modificado = value;
            }
        }
    }
}
