//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SysManagmentCarApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            this.History = new HashSet<History>();
        }
    
        public int id { get; set; }
        public int IdOrder { get; set; }
        public string Descript { get; set; }
        public Nullable<System.DateTime> DateBegin { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<System.TimeSpan> timeStart { get; set; }
        public Nullable<System.TimeSpan> TimeEnd { get; set; }
        public string VinNumber { get; set; }
        public int IdClient { get; set; }
        public Nullable<int> IdEmp { get; set; }
    
        public virtual Cars Cars { get; set; }
        public virtual Clientes Clientes { get; set; }
        public virtual Employess Employess { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<History> History { get; set; }
    }
}
