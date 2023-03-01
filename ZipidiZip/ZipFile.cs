/*
MIT License

Copyright (c) 2019 Fredrik B

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ZipidiZip
{
    public class ZipFile : IDisposable
    {
        private ZipArchive _archive;
        private MemoryStream _stream;

        public ZipFile()
        {
            _stream = new MemoryStream();
            _archive = new ZipArchive(_stream, ZipArchiveMode.Create, leaveOpen: true);
        }

        public ZipFile(Stream stream)
        {
            _stream = new MemoryStream();
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(_stream);
            _archive = new ZipArchive(_stream, ZipArchiveMode.Update, leaveOpen: true);
        }

        public ZipFile(string filePath)
        {
            _stream = new MemoryStream();
            using(var fileStream = new FileStream(filePath, FileMode.Open))
            {
                fileStream.CopyTo(_stream);
            }
            _archive = new ZipArchive(_stream, ZipArchiveMode.Update, leaveOpen: true);
        }

        public async Task AddFileAsync(string fileName, Stream stream)
        {
            var entry = _archive.CreateEntry(fileName);
            using (var entryStream = entry.Open())
            {
                await stream.CopyToAsync(entryStream);
            }
        }

        public void AddFile(string fileName, Stream stream)
        {
            var entry = _archive.CreateEntry(fileName);
            using (var entryStream = entry.Open())
            {
                stream.CopyTo(entryStream);
            }
        }

        public async Task AddFileAsync(string fileName, string filePath)
        {
            var entry = _archive.CreateEntry(fileName);
            using (var entryStream = entry.Open())
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    await fileStream.CopyToAsync(entryStream);
                }
            }
        }

        public void AddFile(string fileName, string filePath)
        {
            var entry = _archive.CreateEntry(fileName);
            using (var entryStream = entry.Open())
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    fileStream.CopyTo(entryStream);
                }
            }
        }

        public void Extract(string directory)
        {
            _archive.ExtractToDirectory(directory);
        }

        public void Save(Stream stream)
        {
            DisposeArchive();
            _stream.Seek(0, SeekOrigin.Begin);
            _stream.CopyTo(stream);
            stream.Seek(0, SeekOrigin.Begin);
        }

        public void Save(string filePath)
        {
            DisposeArchive();
            _stream.Seek(0, SeekOrigin.Begin);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                _stream.CopyTo(fileStream);
            }
        }

        public async Task SaveAsync(Stream stream)
        {
            DisposeArchive();
            _stream.Seek(0, SeekOrigin.Begin);
            await _stream.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
        }

        public async Task SaveAsync(string filePath)
        {
            DisposeArchive();
            _stream.Seek(0, SeekOrigin.Begin);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await _stream.CopyToAsync(fileStream);
            }
        }

        private void DisposeArchive()
        {
            _archive.Dispose();
            _archive = null;
        }

        public void Dispose()
        {
            if (_archive != null)
            {
                _archive.Dispose();
                _archive = null;
            }

            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }
    }
}