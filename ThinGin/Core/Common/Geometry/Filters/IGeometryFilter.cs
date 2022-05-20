namespace ThinGin.Core.Common.Geometry.Filters
{
    /// <summary>
    /// Provides a generic mutation filter for geometry data
    /// </summary>
    public interface IGeometryFilter
    {
        GeometryData Process(GeometryData Geometry, object Context);
    }
}
