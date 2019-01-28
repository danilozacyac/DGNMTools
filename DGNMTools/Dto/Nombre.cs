using System;
using System.Linq;

namespace DGNMTools.Dto
{
    public class Nombre
    {
        private int idNombre;
        private string nombreDesc;
        private string nombreString;
        private int genero;
        private string folio;
        
        public int IdNombre
        {
            get
            {
                return this.idNombre;
            }
            set
            {
                this.idNombre = value;
            }
        }

        public string NombreDesc
        {
            get
            {
                return this.nombreDesc;
            }
            set
            {
                this.nombreDesc = value;
            }
        }

        public string NombreString
        {
            get
            {
                return this.nombreString;
            }
            set
            {
                this.nombreString = value;
            }
        }

        public int Genero
        {
            get
            {
                return this.genero;
            }
            set
            {
                this.genero = value;
            }
        }

        public string Folio
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
    }
}
