﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Yaml.Serialization;

namespace WebSite.App.Speakers
{
    public class SpeakerProvider : IEnumerable<Speaker>
    {
        private readonly List<Speaker> _speakers = new List<Speaker>();

        public SpeakerProvider(DirectoryInfo dataFolder)
        {
            var serializer = new YamlSerializer();
            var profiles = dataFolder.GetFiles("*.yaml");
            foreach (var profileFile in profiles)
            {                
                using (var reader = profileFile.OpenRead())
                {
                    var speaker = (Speaker)serializer.Deserialize(reader, typeof(Speaker))[0];
                    speaker.Id = Path.GetFileNameWithoutExtension(profileFile.Name);
                    _speakers.Add(speaker);
                }
            }
            _speakers.Sort((x, y) => x.FullName.CompareTo(y.FullName));
        }
        
        public IEnumerator<Speaker> GetEnumerator()
        {
            return _speakers.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        public Speaker Get(string id)
        {
            return this.First(x => x.Id == id);
        }
    }
}