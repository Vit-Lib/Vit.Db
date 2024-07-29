namespace Vit.Db.Module.Schema.Extensions
{
    class ColumnSchemaExt : ColumnSchema
    {
        public string table_name { get; set; }
        public string table_schema { get; set; }
    }
}
