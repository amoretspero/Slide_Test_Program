module File2

open System.Drawing
open System.Drawing.Drawing2D

let image_resizer (img : Image) (size : Size) (preserveAspectRatio : bool) =
    /// Setup default value.
    let newWidth = ref 0
    let newHeight = ref 0

    /// Set size with or without Aspect ratio of original one.
    if (preserveAspectRatio = true) then
        let originalWidth = img.Width
        let originalHeight = img.Height
        let percentWidth = size.Width / originalWidth
        let percentHeight = size.Height / originalHeight
        let percent = if (percentHeight < percentWidth) then percentHeight else percentWidth
        newWidth.Value <- originalWidth * percent
        newHeight.Value <- originalHeight * percent
    else
        newWidth.Value <- size.Width
        newHeight.Value <- size.Height

    /// Prepare for new image.
    let newimg = new Bitmap(newWidth.Value, newHeight.Value)
    let graphicsHandle = Graphics.FromImage(newimg)
    graphicsHandle.InterpolationMode <- InterpolationMode.HighQualityBicubic
    graphicsHandle.DrawImage(img, new Rectangle(Point(0, 0), Size(newWidth.Value, newHeight.Value)))
    newimg