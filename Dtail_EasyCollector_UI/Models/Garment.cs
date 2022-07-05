using System.Collections.Generic;

namespace Dtail_EasyCollector_UI.Models
{
    public class Garment
    {
        public Garment()
        {
            HangingMesh = new List<string>();
            CloFiles = new List<string>();
        }

        //Texture repository
        public string DiffuseTexture { get; set; }

        public string NormalTexture { get; set; }

        public string OpacityTexture { get; set; }

        public string MetalnessTexture { get; set; }

        public string RoughnessTexture { get; set; }

        //Mesh repository

        public string HollowBodyMesh { get; set; }

        public List<string> HangingMesh { get; set; }

        public string SquareFoldMesh { get; set; }

        public string LongFoldMesh { get; set; }

        public string HalfFoldMesh { get; set; }

        public string FlatFoldMesh { get; set; }

        //Clo Repository

        public List<string> CloFiles { get; set; }
    }
}
