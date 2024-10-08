﻿using System.Collections.Generic;

namespace Vit.Db.Module.Schema
{
    public class TableSchema
    {
        public string schema_name { get; set; }
        public string table_name { get; set; }
        public List<ColumnSchema> columns { get; set; }
    }
}
