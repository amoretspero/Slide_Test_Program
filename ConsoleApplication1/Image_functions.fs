module Image_functions

open System
open System.Collections
open System.Diagnostics
open System.IO
open System.Linq
open System.Text
open System.Drawing
open System.Drawing.Design
open System.Drawing.Drawing2D
open System.Threading
open System.Windows.Forms

exception DIR_NOT_EXIST

let image_resizer (img : Image) (size : Size) (preserveAspectRatio : bool) =
    /// Setup default value.
    let newWidth = ref 0.0
    let newHeight = ref 0.0

    /// Set size with or without Aspect ratio of original one.
    if (preserveAspectRatio = true) then
        let originalWidth = (float)img.Width
        let originalHeight = (float)img.Height
        let percentWidth = (float)size.Width / (float)originalWidth
        let percentHeight = (float)size.Height / (float)originalHeight
        let percent = if (percentHeight < percentWidth) then percentHeight else percentWidth
        newWidth.Value <- originalWidth * percent
        newHeight.Value <- originalHeight * percent
    else
        newWidth.Value <- (float)size.Width
        newHeight.Value <- (float)size.Height

    /// Prepare for new image.
    let newimg = new Bitmap((int)newWidth.Value, (int)newHeight.Value)
    let graphicsHandle = Graphics.FromImage(newimg)
    graphicsHandle.InterpolationMode <- InterpolationMode.HighQualityBicubic
    graphicsHandle.DrawImage(img, new Rectangle(Point(0, 0), Size((int)newWidth.Value, (int)newHeight.Value)))
    newimg

let rec image_counter () =
    let img_dir = new DirectoryInfo(@"..\..\..\images")
    let img_dir_release = new DirectoryInfo(@".\Images")
    if img_dir.Exists then
        let cnt = img_dir.GetFiles("*.jpg").Length
        cnt
    else if img_dir_release.Exists then
        let cnt = img_dir_release.GetFiles("*.jpg").Length
        cnt
    else
        -1

let rec image_loader () =
    let img_dir = new DirectoryInfo(@"..\..\..\images")
    let img_dir_release = new DirectoryInfo(@".\Images")
    let img_cnt = image_counter()
    if img_dir.Exists then
        let cnt = img_dir.GetFiles("*.jpg").Length
        //let files = img_dir.GetFiles("*.jpg")
        let images = ref ([] : Image list)
        for i=0 to cnt-1 do
            let img = ref ([| |] : FileInfo array)
            if (img_dir.GetFiles((i+1).ToString()+".jpg").Length <> 0) then
                img.Value <- img_dir.GetFiles((i+1).ToString()+".jpg")
            else
                img.Value <- img_dir.GetFiles((i+1).ToString()+"_one.jpg")
            let fullname = img.Value.[0].FullName
            let image = Image.FromFile(fullname)
            images.Value <- (List.append images.Value [image])
        images.Value
    else if img_dir_release.Exists then
        let cnt = img_dir_release.GetFiles("*.jpg").Length
        let images = ref ([] : Image list)
        for i=0 to cnt-1 do
            let img = ref ([| |] : FileInfo array)
            if (img_dir_release.GetFiles((i+1).ToString()+".jpg").Length <> 0) then
                img.Value <- img_dir_release.GetFiles((i+1).ToString()+".jpg")
            else
                img.Value <- img_dir_release.GetFiles((i+1).ToString()+"_one.jpg")
            let fullname = img.Value.[0].FullName
            let image = Image.FromFile(fullname)
            images.Value <- (List.append images.Value [image])
        images.Value
    else
        raise DIR_NOT_EXIST


/// show_image_in_form : given center point that image to be displayed, boundary(in width, height), img, form, this function will return form that given image is displayed.
let rec show_image_in_form (p : Point) (w : int) (h : int) (img : Image) (f : Form) =
    let width = img.Width
    let height = img.Height
    let image_resized = (image_resizer img (new Size(w, h)) true)
    let width_resized = image_resized.Width
    let height_resized = image_resized.Height
    let image_box = new PictureBox(Width = w, Height = h, Location = new Point((p.X - (width_resized/2)), (p.Y - (height_resized/2))), Name = "image_box")
    image_box.Image <- image_resized
    f.Controls.Add(image_box)
    f

/// remove_image_in_form : given form, find controls that have name of "image_box"(added to controls at show_image_in_form function) and remove them all. Return given form with image_boxes removed.
let rec remove_image_in_form (f : Form) = 
    let controls = f.Controls
    let image_controls = controls.Find("image_box", true)
    for elem in image_controls do
        f.Controls.Remove(elem)
        elem.Dispose()
    f.Refresh()
    f
