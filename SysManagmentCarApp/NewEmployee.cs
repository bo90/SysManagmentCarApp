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
    
    public partial class NewEmployee
    {
        public int id { get; set; }
        public int idEmp { get; set; }
        public string nameEmp { get; set; }
        public string sName { get; set; }
        public string enducation { get; set; }
        public string enduPlace { get; set; }
        public string originCity { get; set; }
        public Nullable<System.DateTime> age { get; set; }
        public string profession { get; set; }
        public string telephone { get; set; }
        public string email { get; set; }
    
        public virtual Employess Employess { get; set; }
    }
}