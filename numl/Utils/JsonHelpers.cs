﻿using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections.Generic;

namespace numl.Utils
{
    public static class JsonHelpers
    {
        /// <summary>Save object to file.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="file">file.</param>
        /// <param name="o">object.</param>
        ///
        /// ### <typeparam name="T">Type.</typeparam>
        public static void Save<T>(string file, T o)
        {
            Save(file, o, typeof(T));
        }

        /// <summary>Save object to file.</summary>
        /// <param name="file">file.</param>
        /// <param name="o">object.</param>
        /// <param name="t">type.</param>
        public static void Save(string file, object o, Type t)
        {
            using (var stream = File.OpenWrite(file))
            using (var writer = new StreamWriter(stream))
                Save(writer, o, t);
        }

        /// <summary>Save object to file.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="stream">The stream.</param>
        /// <param name="o">object.</param>
        public static void Save<T>(TextWriter writer, T o)
        {
            Save(writer, o, typeof(T));
        }

        /// <summary>Save object to file.</summary>
        /// <param name="writer">The stream.</param>
        /// <param name="o">object.</param>
        /// <param name="t">type.</param>
        public static void Save(TextWriter writer, object o, Type t)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, o, t);
        }

        /// <summary>Converts an o to an json string.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="o">object.</param>
        /// <returns>o as a string.</returns>
        public static string ToJsonString<T>(T o)
        {
            return ToJsonString(o, typeof(T));
        }

        /// <summary>Converts this object to an json string.</summary>
        /// <param name="o">object.</param>
        /// <param name="t">type.</param>
        /// <returns>The given data converted to a string.</returns>
        public static string ToJsonString(object o, Type t)
        {
            using (StringWriter textWriter = new StringWriter())
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(textWriter, o, t);
                return textWriter.ToString();
            }
        }

        /// <summary>Loads the given stream.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="file">file.</param>
        /// <returns>A T.</returns>
        public static T Load<T>(string file)
        {
            return (T)Load(file, typeof(T));
        }

        /// <summary>Loads.</summary>
        /// <param name="file">file.</param>
        /// <param name="t">type.</param>
        /// <returns>An object.</returns>
        public static object Load(string file, Type t)
        {
            using (var stream = File.OpenRead(file))
            using (var reader = new StreamReader(stream))
                return Load(reader, t);
        }

        /// <summary>Loads the given stream.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="stream">The stream.</param>
        /// <returns>A T.</returns>
        public static T Load<T>(TextReader reader)
        {
            return (T)Load(reader, typeof(T));
        }
        /// <summary>Loads.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="t">type.</param>
        /// <returns>An object.</returns>
        public static object Load(TextReader reader, Type t)
        {
            JsonSerializer serializer = new JsonSerializer();
            return serializer.Deserialize(reader, t);
        }

        /// <summary>Loads json string.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="json">The json.</param>
        /// <returns>The json string.</returns>
        public static T LoadJsonString<T>(string json)
        {
            return (T)LoadJsonString(json, typeof(T));
        }

        /// <summary>Loads json string.</summary>
        /// <param name="json">The JSONHelpers.</param>
        /// <param name="t">type.</param>
        /// <returns>The json string.</returns>
        public static object LoadJsonString(string json, Type t)
        {
            using (StringReader textReader = new StringReader(json))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize(textReader, t);
            }
        }

        /// <summary>Writes.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="writer">The writer.</param>
        /// <param name="thing">The thing.</param>
        public static void Write<T>(TextWriter writer, T thing)
        {
            JsonSerializer serializer = new JsonSerializer();

            // check for a null thing
            if (thing != null)
                serializer.Serialize(writer, thing);
            else
            {
                var ctor = typeof(T).GetConstructor(new Type[] { });
                if (ctor != null)
                    serializer.Serialize(writer, ctor.Invoke(new object[] { }));
                else
                    serializer.Serialize(writer, default(T));
            }
        }

        /// <summary>Reads the given reader.</summary>
        /// <tparam name="T">Generic type parameter.</tparam>
        /// <param name="reader">The reader.</param>
        /// <returns>A T.</returns>
        public static T Read<T>(TextReader reader)
        {
            JsonSerializer serializer = new JsonSerializer();
            return (T)serializer.Deserialize(reader, typeof(T));
        }
    }
}