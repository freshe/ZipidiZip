Small zip wrapper around System.IO.Compression for .NET

```cs
using (var zip = new ZipFile())
{
	zip.AddFile("Picture1.jpg", @"/Pictures/Pic001.jpg");
	zip.AddFile("Picture2.jpg", @"/Pictures/Pic002.jpg");
	zip.AddFile("Picture3.jpg", @"/Pictures/Pic003.jpg");

	zip.Save(@"/Pictures/Archive.zip");
}
```

```cs
var stream = new MemoryStream();

using (var zip = new ZipFile())
{
	zip.AddFile("Picture1.jpg", pictureStream1);
	zip.AddFile("Picture2.jpg", pictureStream2);
	zip.AddFile("Picture3.jpg", pictureStream3);

	zip.Save(stream);
}
```