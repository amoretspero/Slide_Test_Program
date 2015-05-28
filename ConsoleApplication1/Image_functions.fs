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
    let img_dir = new DirectoryInfo(@"..\..\..\Slide_Test_Program_Images")
    let img_dir_release = new DirectoryInfo(@".\Slide_Test_Program_Images")
    if img_dir.Exists then
        let cnt = img_dir.GetFiles("*.jpg").Length
        cnt
    else if img_dir_release.Exists then
        let cnt = img_dir_release.GetFiles("*.jpg").Length
        cnt
    else
        -1

let sort_img (img1 : FileInfo) (img2 : FileInfo) =
    let img1_name = img1.Name
    let img2_name = img2.Name
    let mutable img1_num = img1_name.Substring(0, img1_name.IndexOf("."))
    let mutable img2_num = img2_name.Substring(0, img2_name.IndexOf("."))
    if (img1_name.Contains("-")) then img1_num <- img1_name.Substring(0, img1_name.IndexOf("-"))
    if (img2_name.Contains("-")) then img2_num <- img2_name.Substring(0, img2_name.IndexOf("-"))
    let img1_num_final = Convert.ToInt32(img1_num)
    let img2_num_final = Convert.ToInt32(img2_num)
    if img1_num_final < img2_num_final then 
        -1
    else if img1_num_final = img2_num_final then
        0
    else
        1

/// When called, returns int list list, whose each element is [image_number; image_count].
let rec get_plural_images () =
    let img_dir = new DirectoryInfo(@"../../../Slide_Test_Program_Images")
    let img_dir_release = new DirectoryInfo(@"./Slide_Test_Program_Images")
    if img_dir.Exists then
        let cnt_all = img_dir.GetFiles("*.jpg").Length
        let images_unsorted = img_dir.GetFiles("*.jpg")
        let images = List.sortWith (fun img1 img2 -> sort_img img1 img2) (Array.toList(images_unsorted))
        let real_num_list = ref ([] : int list list)
        let mutable img_per_realnum = 0
        let mutable prev_real_num = -1
        let mutable real_num = -1
        for i=0 to cnt_all-1 do
            prev_real_num <- real_num
            let img = images.[i]
            if img.Name.Contains("-") then
                real_num <- Convert.ToInt32(img.Name.Substring(0, img.Name.IndexOf("-")))
                if real_num <> prev_real_num then
                    if (prev_real_num > 0) then
                        real_num_list.Value <- List.append (real_num_list.Value) [[prev_real_num; img_per_realnum]]
                    img_per_realnum <- 1
                else
                    img_per_realnum <- img_per_realnum + 1
            else
                if prev_real_num > 0 then
                    real_num_list.Value <- List.append (real_num_list.Value) [[prev_real_num; img_per_realnum]]
                real_num <- Convert.ToInt32(img.Name.Substring(0, img.Name.IndexOf(".")))
                img_per_realnum <- 1
        real_num_list.Value <- List.append (real_num_list.Value) [[prev_real_num; img_per_realnum]]
        real_num_list.Value
    else if img_dir_release.Exists then
        let cnt_all = img_dir_release.GetFiles("*.jpg").Length
        let images_unsorted = img_dir_release.GetFiles("*.jpg")
        let images = List.sortWith (fun img1 img2 -> sort_img img1 img2) (Array.toList(images_unsorted))
        let real_num_list = ref ([] : int list list)
        let mutable img_per_realnum = 0
        let mutable prev_real_num = -1
        let mutable real_num = -1
        for i=0 to cnt_all-1 do
            prev_real_num <- real_num
            let img = images.[i]
            if img.Name.Contains("-") then
                real_num <- Convert.ToInt32(img.Name.Substring(0, img.Name.IndexOf("-")))
                if real_num <> prev_real_num then
                    if (prev_real_num > 0) then
                        real_num_list.Value <- List.append (real_num_list.Value) [[prev_real_num; img_per_realnum]]
                    img_per_realnum <- 1
                else
                    img_per_realnum <- img_per_realnum + 1
            else
                if prev_real_num > 0 then
                    real_num_list.Value <- List.append (real_num_list.Value) [[prev_real_num; img_per_realnum]]
                real_num <- Convert.ToInt32(img.Name.Substring(0, img.Name.IndexOf(".")))
                img_per_realnum <- 1
        real_num_list.Value <- List.append (real_num_list.Value) [[prev_real_num; img_per_realnum]]
        real_num_list.Value
    else
        ([] : int list list)

/// get_images : When called with parameters, returns list of images that contains primary number which equals with real_num.
let rec get_images (real_num : int) (count : int) =
    let img_dir = new DirectoryInfo(@"../../../Slide_Test_Program_Images")
    let img_dir_release = new DirectoryInfo(@"./Slide_Test_Program_Images")
    if img_dir.Exists then
        if (count = 1) then
            let images = ref ([] : Image list)
            let img = img_dir.GetFiles(real_num.ToString()+".jpg")
            let fullname = img.[0].FullName
            let image = Image.FromFile(fullname)
            images.Value <- (List.append images.Value [image])
            images.Value
        else
            let images = ref ([] : Image list)
            for i=1 to count do
                let img = img_dir.GetFiles(real_num.ToString()+"-"+i.ToString()+".jpg")
                let fullname = img.[0].FullName
                let image = Image.FromFile(fullname)
                images.Value <- (List.append images.Value [image])
            images.Value
    else if img_dir_release.Exists then
        if (count = 1) then
            let images = ref ([] : Image list)
            let img = img_dir_release.GetFiles(real_num.ToString()+".jpg")
            let fullname = img.[0].FullName
            let image = Image.FromFile(fullname)
            images.Value <- (List.append images.Value [image])
            images.Value
        else
            let images = ref ([] : Image list)
            for i=1 to count do
                let img = img_dir_release.GetFiles(real_num.ToString()+"-"+i.ToString()+".jpg")
                let fullname = img.[0].FullName
                let image = Image.FromFile(fullname)
                images.Value <- (List.append images.Value [image])
            images.Value
    else
        ([] : Image list)

let rec image_loader () =
    let img_dir = new DirectoryInfo(@"..\..\..\Slide_Test_Program_Images")
    let img_dir_release = new DirectoryInfo(@".\Slide_Test_Program_Images")
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

let rec show_image_in_form_plural (p : Point list) (w : int) (h : int) (img : Image list) (f : Form) =
    let cnt = img.Count()
    for i = 0 to cnt-1 do
        let point = p.[i]
        let image = img.[i]
        show_image_in_form point w h image f |> ignore

/// remove_image_in_form : given form, find controls that have name of "image_box"(added to controls at show_image_in_form function) and remove them all. Return given form with image_boxes removed.
let rec remove_image_in_form (f : Form) = 
    let controls = f.Controls
    let image_controls = controls.Find("image_box", true)
    for elem in image_controls do
        f.Controls.Remove(elem)
        elem.Dispose()
    f.Refresh()
    f

// Point list, boundary width, boundary height setting.
let point_list_for_one = [(new Point(500, 300))]
let boundary_width_for_one = 640
let boundary_height_for_one = 480
let point_list_for_two = [(new Point(300, 300)); (new Point(700, 300))]
let point_list_for_two_vertical = [(new Point(500, 200)); (new Point(500, 400))]
let boundary_width_for_two = 400
let boundary_height_for_two = 500
let boundary_width_for_two_vertical = 600
let boundary_height_for_two_vertical = 200
let point_list_for_three = [(new Point(200, 300)); (new Point(500, 300)); (new Point(800, 300))]
let point_list_for_three_vertical = [(new Point(500, 150)); (new Point(500, 300)); (new Point(500, 450))]
let boundary_width_for_three = 300
let boundary_height_for_three = 500
let boundary_width_for_three_vertical = 600
let boundary_height_for_three_vertical = 150

/// show_image_in_form_plural_gen : given number of images, list of images, appropriate form, show that images in given form with horizental or vertical position.
let show_image_in_form_plural_gen (cnt : int) (images : Image list) (form : Form) =
    if (cnt = 1) then 
        (show_image_in_form_plural point_list_for_one boundary_width_for_one boundary_height_for_one images form)
    else if (cnt = 2) then 
        if ((float)images.[0].Width / (float)images.[0].Height >= 2.5) then
            (show_image_in_form_plural point_list_for_two_vertical boundary_width_for_two_vertical boundary_height_for_two_vertical images form)
        else
            (show_image_in_form_plural point_list_for_two boundary_width_for_two boundary_height_for_two images form)
    else 
        if ((float)images.[0].Width / (float)images.[0].Height >= 2.5) then
            (show_image_in_form_plural point_list_for_three_vertical boundary_width_for_three_vertical boundary_height_for_three_vertical images form)
        else
            (show_image_in_form_plural point_list_for_three boundary_width_for_three boundary_height_for_three images form)
