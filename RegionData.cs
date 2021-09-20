using System.Numerics;

namespace generate_terrain
{
    public class RegionData
    {
        protected float m_radius = 0;
        protected Vector<int> m_center;
        protected int m_index;
        protected int m_max_rsquared;
        protected uint m_color;


        public int Index => m_index;
        public Vector<int> Center => m_center;
        public float Radius => m_radius;
        public bool IsGrowing => (m_radius * m_radius) < m_max_rsquared;
        public uint Color => m_color;
    }
}