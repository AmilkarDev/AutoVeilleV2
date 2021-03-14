using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Outils
{
    /// <summary>
    /// Extention pour les classes system de type stream
    /// </summary>
    public static class StreamExtentions
    {
        /// <summary>
        /// Lit tout les byte d'un stream binaire
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                var buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }

        }

    }
}

