using System;
using System.Linq;

namespace DGNMTools.Dto
{
    public class ValidaSocios
    {
        private int idSocio;
        private string nombre;
        private string paterno;
        private string materno;
        private string fcNacimiento;
        private string rfc;
        private string curp;
        private bool rfcCorrect = false;
        private bool curpCorrect = false;
        private bool identMatch = false;
        private bool fnacCorrect = false;
        private string fNacUpdt;
        private bool factors4 = false;

        public int IdSocio
        {
            get
            {
                return this.idSocio;
            }
            set
            {
                this.idSocio = value;
            }
        }

        public string Nombre
        {
            get
            {
                return this.nombre;
            }
            set
            {
                this.nombre = value;
            }
        }

        public string Paterno
        {
            get
            {
                return this.paterno;
            }
            set
            {
                this.paterno = value;
            }
        }

        public string Materno
        {
            get
            {
                return this.materno;
            }
            set
            {
                this.materno = value;
            }
        }

        public string FcNacimiento
        {
            get
            {
                return this.fcNacimiento;
            }
            set
            {
                this.fcNacimiento = value;
            }
        }

        public string Rfc
        {
            get
            {
                return this.rfc;
            }
            set
            {
                this.rfc = value;
            }
        }

        public string Curp
        {
            get
            {
                return this.curp;
            }
            set
            {
                this.curp = value;
            }
        }

        public bool RfcCorrect
        {
            get
            {
                return this.rfcCorrect;
            }
            set
            {
                this.rfcCorrect = value;
            }
        }

        public bool CurpCorrect
        {
            get
            {
                return this.curpCorrect;
            }
            set
            {
                this.curpCorrect = value;
            }
        }

        public bool IdentMatch
        {
            get
            {
                return this.identMatch;
            }
            set
            {
                this.identMatch = value;
            }
        }

        public bool FnacCorrect
        {
            get
            {
                return this.fnacCorrect;
            }
            set
            {
                this.fnacCorrect = value;
            }
        }

        public string FNacUpdt
        {
            get
            {
                return this.fNacUpdt;
            }
            set
            {
                this.fNacUpdt = value;
            }
        }

        public bool Factors4
        {
            get
            {
                return this.factors4;
            }
            set
            {
                this.factors4 = value;
            }
        }

        
    }
}
