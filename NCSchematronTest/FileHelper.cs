using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace NCSchematronTest
{
    public class Resolver : XmlUrlResolver
    {
        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            Uri result = null;
            if (baseUri != null)
            {
                if (!String.IsNullOrEmpty(relativeUri))
                {
                    string file = FileHelper.FindSchema(relativeUri);
                    if (file != null)
                    {
                        result = new Uri("file://" + file);
                    }
                }
                else if (File.Exists(baseUri.LocalPath))
                {
                    result = baseUri;
                }
            }
            else if (!String.IsNullOrEmpty(relativeUri))
            {
                string file = FileHelper.FindSchema(relativeUri);
                if (file != null)
                {
                    result = new Uri("file://" + file);
                }
            }

            if (result == null)
            {
                result = base.ResolveUri(baseUri, relativeUri);
            }
            return result;
        }
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            object entity = base.GetEntity(absoluteUri, role, ofObjectToReturn);
            return entity;
        }
    }
    public static class FileHelper
    {
        public static Resolver Resolver => new Resolver();
        public static string FindPath(string name)
        {
            if (File.Exists(name))
            {
                return name;
            }
            var workingDir = Directory.GetCurrentDirectory();
            
            if (File.Exists(Path.Combine(workingDir, name)))
            {
                return Path.Combine(workingDir, name);
            }

            workingDir = Path.GetDirectoryName(workingDir);
            if (workingDir == null) return null;
            do
            {
                if (File.Exists(Path.Combine(workingDir, name)))
                {
                    return Path.Combine(workingDir, name);
                }
                workingDir = Path.GetDirectoryName(workingDir);
            } while (workingDir != null);
            return null;
        }
        public static string FindSchema(string name)
        {
            return FindPath(Path.Combine("Schemas", name));
        }
        public static string FindDocument(string name)
        {
            return FindPath(Path.Combine("Documents", name));
        }
        public static string FindCCD(string name)
        {
            return FindPath(Path.Combine("Documents", "CCD", name));
        }
    }
}
