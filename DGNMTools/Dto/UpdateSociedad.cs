using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGNMTools.Dto
{
    public class UpdateSociedad
    {
        private int caratula;
        private string folio;
        private string denominacion;
        private int estado;
        private int municipio;
        private int oficina;
        private int idTipoSociedad;
        private string subtipoSociedad;
        private string updateString;

        public UpdateSociedad(int caratula, string folio, string denominacion, int estado, int municipio, int oficina, int idTipoSociedad, string subtipoSociedad)
        {
            this.caratula = caratula;
            this.folio = folio;
            this.denominacion = denominacion;
            this.estado = estado;
            this.municipio = municipio;
            this.oficina = oficina;
            this.idTipoSociedad = idTipoSociedad;
            this.subtipoSociedad = subtipoSociedad;
            this.SetUpdateTipoSociedad();
        }


        public string UpdateString
        {
            get
            {
                return this.updateString;
            }
            set
            {
                this.updateString = value;
            }
        }

        private void SetUpdateTipoSociedad()
        {
            updateString = String.Format("UPDATE mvcaratulas mc set mc.lltiposociedad = {0}, mc.dstiposociedad = '{1}'  WHERE mc.llcaratula= {2} and mc.crfme= '{3}' and mc.llestado = {4} and mc.llmunicipio= {5} and mc.lloficina= {6} and mc.dsdensocial= '{7}'; commit;",
                this.idTipoSociedad,this.subtipoSociedad,this.caratula,this.folio,this.estado,this.municipio,this.oficina,this.denominacion);
        }

    }
}
