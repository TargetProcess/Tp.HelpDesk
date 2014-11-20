using System;
using System.Xml.Serialization;

namespace Hd.Web.REST
{


    [XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "Projects")]
    public class ProjectCollection
    {
        public ProjectCollection() { Projects = new Project[0]; }

        [XmlElement("Project")]
        public Project[] Projects { get; set; }

        [XmlAttribute]
        public string Next { get; set; }
    }

    [XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "Project")]
    public class Project
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement]
        public Boolean IsActive { get; set; }

        [XmlElement]
        public Boolean IsProduct { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, IsActive: {2}, IsProduct: {3}", Id, Name, IsActive, IsProduct);
        }
    }

}