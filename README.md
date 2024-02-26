# Picturify

Picturify is a C# library to edit images.


## Usage/Examples

After running Install-Picturify-Cli.ps1 you can use cli version by running
```powershell
PicturifyCli --help
```

Or you can use it inside C# application:

```cs
var file = "path_to_image";
var fastImage = FastImageFactory.FromFile(file);
fastImage.ExecuteProcessor(new DualOperatorProcessor(new DualOperatorParams(ChannelSelector.RGB, new SobelOperator5(), OperatorBeforeNormalizationFunc.Log)));
fastImage.Save(file);
```
## Features
Available filters in Core project:
- gaussian blur
- maximum blur
- mean blur
- median blur
- min blur
- dual operator (to use with for example PrewittOperator3)
- quad operator (to use with for example SobelOperator3)
- non maximum gradient suppression
- gamma correction
- negative
- sepia
- multiple convolution filters

Movie project allows you to define IMovieTransform which will result in conversion of a movie.

More features to come!
