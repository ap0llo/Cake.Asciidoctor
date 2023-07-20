# Cake.Asciidoctor

Cake.Asciidoctor is a [Cake](https://cakebuild.net/) Addin that provides aliases for [Asciidoctor](https://docs.asciidoctor.org/asciidoctor/latest/) and [Asciidoctor PDF](https://docs.asciidoctor.org/pdf-converter/latest/)

You can read the latest documentation at https://github.com/ap0llo/Cake.Asciidoctor.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Usage](#usage)
- [License](#license)

## Prerequisites

Asciidoctor and/or Asciidoctor PDF need to be installed separately for the Cake addin to work.  
Please follow the official documentation for installation instructions:

- [Install Asciidoctor](https://docs.asciidoctor.org/asciidoctor/latest/install/)
- [Install Asciidoctor PDF](https://docs.asciidoctor.org/pdf-converter/latest/install/)

## Usage

### Install the Addin

Install the addin into your Cake build.

- If you are using Cake scripting, use the `#addin` reprocessor directive:

  ```cs
  #addin nuget:?package=Cake.Asciidoctor&version=VERSION
  ```

- If you are using [Cake.Frosting](https://cakebuild.net/docs/running-builds/runners/cake-frosting), install the addin by adding a package reference to your project:

  ```xml
  <PackageReference Include="Cake.Asciidoctor" Version="VERSION" /> 
  ```

### Converting an AsciiDoc document to HTML

The `Asciidoctor()` alias runs Asciidoctor to convert an AsciiDoc document to HTML.

The only required parameter is the path of the input document.
This will convert the document to HTML and place it next to the input document:

```cs
Task("ConvertDocuments").Does(() =>
{
    // This will create "input.html" in the same directory as input.adoc
    Asciidoctor("input.adoc");
});

```

Additional (optional) settings can be specified using the `AsciidoctorSettings` parameter.

For example, to change the output directory, use the `DestinationDirectory` property:

```cs
Task("ConvertDocuments").Does(() =>
{
    Asciidoctor(
        "input.adoc",
        new AsciidoctorSettings() 
        {
            DestinationDirectory = "./output-directory"
        });
});
```

The avaialble options in `AsciidoctorSettings` correspond to the command line options of Asciidoctor.
Please refer to [asciidoctor(1)](https://docs.asciidoctor.org/asciidoctor/latest/cli/man1/asciidoctor/) for detailed information.

### Converting an AsciiDoc document to PDF

The `AsciidoctorPdf()` alias runs Asciidoctor PDF to convert an AsciiDoc document to PDF.

The only required parameter is the path of the input document.
This will convert the document to a PDF and place it next to the input document:

```cs
Task("ConvertDocuments").Does(() =>
{
    // This will create "input.html" in the same directory as input.adoc
    AsciidoctorPdf("input.adoc");
});

```

Additional (optional) settings can be specified using the `AsciidoctorPdfSettings` parameter.

For example, to change the output directory, use the `DestinationDirectory` property:

```cs
Task("ConvertDocuments").Does(() =>
{
    AsciidoctorPdf(
        "input.adoc",
        new AsciidoctorPdfSettings() 
        {
            DestinationDirectory = "./output-directory"
        });
});
```

The available options in `AsciidoctorPdfSettings` correspond to the command line options of Asciidoctor PDF and are mostly the same as the options of Asciidoctor.
Please refer to the [Asciidoctor PDF documentation](https://docs.asciidoctor.org/pdf-converter/latest/) for details.

## License

Cake.Asciidoctor is licensed under the MIT License.

For details see https://github.com/ap0llo/Cake.Asciidoctor/blob/main/LICENSE