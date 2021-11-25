
using SqlSugar;

namespace Moz.Bus.Models.Common
{
    [SugarTable("tab_identify")]
    public class Identify:BaseModel
    {
        public bool Rel { get; set; }
    }
}