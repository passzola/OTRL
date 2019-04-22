using System;
using System.Collections.Generic;
using System.Text;

namespace CUSTOR.OTRLS.Core
{
    public partial class StaticData
    {
    
        public int Id { get; set; }
        public string Description { get; set; }
    }

    // For entities whose Id is string
    public partial class StaticData2
    {

        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Description { get; set; }
    }
}
