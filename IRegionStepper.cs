namespace generate_terrain
{
    public interface IRegionStepper
    {
        void Step(Terrain terrain, RegionData region);
    }
}