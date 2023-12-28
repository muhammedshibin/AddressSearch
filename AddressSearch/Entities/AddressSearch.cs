public static DataTable ConvertToDataTable<T>(List<T> list)
{
    DataTable dt = new DataTable();
    foreach (var prop in typeof(T).GetProperties())
    {
        dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
    }
    foreach (var item in list)
    {
        DataRow dr = dt.NewRow();
        foreach (var prop in typeof(T).GetProperties())
        {
            dr[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        }
        dt.Rows.Add(dr);
    }
    return dt;
}
