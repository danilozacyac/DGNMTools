using System;
using System.Linq;

namespace DGNMTools.MigracionDf
{
    public class DataFromTexto
    {
        private int id;
        private int folio;
        private string texto;
        private string textoStr;
        private int numFedObt;
        private string municipioObt;
        private string estadoObt;
        private string fedatarioObt;
        private string tipoFedatarios;
        private int esConstitucion;
        private string permisoRelex;
        private string fechaPermisoRelex;
        private string expedienteRelex;
        private string capitalSocial;
        private int capitalSocialInt;
        private string numAcciones;
        private int numeroAcciones;
        private string valorAcciones;
        private int valorIntAcciones;
        private string textoValorAccion;

        

        public string TextoValorAccion
        {
            get
            {
                return this.textoValorAccion;
            }
            set
            {
                this.textoValorAccion = value;
            }
        }

        public string CapitalSocial
        {
            get
            {
                return this.capitalSocial;
            }
            set
            {
                this.capitalSocial = value;
            }
        }

        public int CapitalSocialInt
        {
            get
            {
                return this.capitalSocialInt;
            }
            set
            {
                this.capitalSocialInt = value;
            }
        }

        public string NumAcciones
        {
            get
            {
                return this.numAcciones;
            }
            set
            {
                this.numAcciones = value;
            }
        }

        public int NumeroAcciones
        {
            get
            {
                return this.numeroAcciones;
            }
            set
            {
                this.numeroAcciones = value;
            }
        }

        public string ValorAcciones
        {
            get
            {
                return this.valorAcciones;
            }
            set
            {
                this.valorAcciones = value;
            }
        }

        public int ValorIntAcciones
        {
            get
            {
                return this.valorIntAcciones;
            }
            set
            {
                this.valorIntAcciones = value;
            }
        }

        public string PermisoRelex
        {
            get
            {
                return this.permisoRelex;
            }
            set
            {
                this.permisoRelex = value;
            }
        }

        public string FechaPermisoRelex
        {
            get
            {
                return this.fechaPermisoRelex;
            }
            set
            {
                this.fechaPermisoRelex = value;
            }
        }

        public string ExpedienteRelex
        {
            get
            {
                return this.expedienteRelex;
            }
            set
            {
                this.expedienteRelex = value;
            }
        }

        public string TipoFedatarios
        {
            get
            {
                return this.tipoFedatarios;
            }
            set
            {
                this.tipoFedatarios = value;
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

        public int Folio
        {
            get
            {
                return this.folio;
            }
            set
            {
                this.folio = value;
            }
        }

        public string Texto
        {
            get
            {
                return this.texto;
            }
            set
            {
                this.texto = value;
            }
        }

        public string TextoStr
        {
            get
            {
                return this.textoStr;
            }
            set
            {
                this.textoStr = value;
            }
        }

        public int NumFedObt
        {
            get
            {
                return this.numFedObt;
            }
            set
            {
                this.numFedObt = value;
            }
        }

        public string MunicipioObt
        {
            get
            {
                return this.municipioObt;
            }
            set
            {
                this.municipioObt = value;
            }
        }

        public string EstadoObt
        {
            get
            {
                return this.estadoObt;
            }
            set
            {
                this.estadoObt = value;
            }
        }

        public string FedatarioObt
        {
            get
            {
                return this.fedatarioObt;
            }
            set
            {
                this.fedatarioObt = value;
            }
        }

        public int EsConstitucion
        {
            get
            {
                return this.esConstitucion;
            }
            set
            {
                this.esConstitucion = value;
            }
        }
    }
}
