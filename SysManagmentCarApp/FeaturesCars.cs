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
    
    public partial class FeaturesCars
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FeaturesCars()
        {
            this.History = new HashSet<History>();
        }
    
        public int Id { get; set; }
        public string VinNumber { get; set; }
        public string PTC_number { get; set; }
        public string Color { get; set; }
        public Nullable<double> EngienPower { get; set; }
        public string TypeEngien { get; set; }
        public Nullable<double> VolumeEngien { get; set; }
        public string TypeDriveMachine { get; set; }
        public string TypeFrame { get; set; }
        public string Category { get; set; }
        public Nullable<int> Age { get; set; }
    
        public virtual Cars Cars { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<History> History { get; set; }
    }
}
