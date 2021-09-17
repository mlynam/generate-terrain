namespace generate_terrain
{
    public class Seed
    {
        int m_size;
        int m_region_count;
        int m_region_growth_rate;


        public Seed(int seed)
        {
            m_size = (int)(byte)seed >> 8;
            m_region_count = (int)(byte)seed >> 4;
            m_region_growth_rate = (int)(byte)seed;
        }

        public Seed(int size, int region_count, int region_growth_rate)
        {
            m_size = size;
            m_region_count = region_count;
            m_region_growth_rate = region_growth_rate;
        }

        public int Pack()
        {
            return m_size << 16 | m_region_count << 8 | m_region_growth_rate;
        }

        public int Size => m_size;
        public int RegionCount => m_region_count;
        public int RegionGrowthRate => m_region_growth_rate;
    }
}