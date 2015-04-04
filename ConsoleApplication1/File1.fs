open System
open System.Collections
open System.Diagnostics
open System.IO
open System.Linq
open System.Text
open System.Drawing
open System.Drawing.Design
open System.Threading
open System.Windows.Forms

open Image_functions

/// Fonts. - Must include : font name, font size, FontStyle, GraphicsUnit
let font_title = new Font("나눔손글씨 펜", 14.0f, FontStyle.Regular, GraphicsUnit.Point)
let font_text = new Font("맑은 고딕", 10.0f, FontStyle.Regular, GraphicsUnit.Point)
let font_label = new Font("서울남산체", 10.0f, FontStyle.Regular, GraphicsUnit.Point)

/// Forms for slide_test, test_selections. - Must include : Text, Width, Height, StartPosition, AutoScaleMode, FormBorderStyle
let form_slide_test = new Form(Width = 1280, Height = 720, StartPosition = FormStartPosition.CenterScreen, AutoScaleMode = AutoScaleMode.Font, FormBorderStyle = FormBorderStyle.FixedSingle, Text = "Slide Test")

/// Labels - Must include : Text, Width, Height, 
let label_image = new Label(Text = "Image", Width = 100, Height = 20, Location = new Point(600, 20), Font = font_label)
let label_answer = new Label(Text = "Answer :", Width = 70, Height = 20, Location = new Point(100, 650), Font = font_label)
let label_correctness = new Label(Text = "Correctness : ", Width = 100, Height = 20, Location = new Point(400, 650), Font = font_label)
let label_correctness_answer = new Label(Text = "N\A", Width = 100, Height = 20, Location = new Point(500, 650), Font = font_label)
let label_correctanswer = new Label(Text = "Correct Answer :", Width = 160, Height = 20, Location = new Point(480, 600), Font = font_label)
let label_correctanswer_answer = new Label(Text = "N/A", Width = 100, Height = 20, Location = new Point(650, 600), Font = font_text)


/// Buttons - Must include : Text, Width, Height, Location, Font
let button_close = new Button(Text = "Close", Width = 70, Height = 25, Location = new Point(900, 645), Font = font_label)
let button_next_image = new Button(Text = "Next Image", Width = 120, Height = 25, Location = new Point(620, 550), Font = font_label)
let button_previous_image = new Button(Text = "Prev Image", Width = 120, Height = 25, Location = new Point(480, 550), Font = font_label)
let button_answer_submit = new Button(Text = "Submit", Width = 80, Height = 25, Location = new Point(300, 645), Font = font_label)
let button_reset_random_list = new Button(Text = "Random Order Reset", Width = 170, Height = 25, Location = new Point(1000, 645), Font = font_label)
let button_start = new Button(Text = "Test Start", Width = 120, Height = 25, Location = new Point(1000, 25), Font = font_label)

/// Text boxes - Must include : Text, Width, Height, Location, Font
let textbox_answer = new TextBox(Text = "", Width = 120, Height = 25, Location = new Point(170, 645), Font = font_text)

/// Form control adder.
let rec form_control_adder (f : Form) (lst : Control list) =
    match lst with
    [] -> f
    | h :: t -> 
        f.Controls.Add(h)
        (form_control_adder f t)

/// Random list generator - Generates list of random numbers, 0 to (num-1), no duplicates.
let rec random_list_gen (num : int) (lst : int list) =
    match lst with
    [] -> (random_list_gen num [ (Random().Next())%num ])
    | h :: t ->
        if (lst.Length <> num) then
            let mutable temp = (Random().Next())%num
            while (lst.Contains(temp)) do
                temp <- (Random().Next())%num
            (random_list_gen num (List.append lst [temp]))
        else
            lst

/// Imaged loaded from specified folder. Only finds images with format JPEG(.jpg)
let images = image_loader()
let images_count = image_counter()

/// Generate random list for images.
let random_list_for_images = (random_list_gen images_count [])

/// Image counter
let mutable image_counter = 0

/// Event handlers.
button_close.Click.Add(fun _ ->
    form_slide_test.Close())

button_start.Click.Add(fun _ ->
    if (image_counter = 0) then
        let image_num = random_list_for_images.[0]
        (show_image_in_form (new Point(640, 300)) 640 480 (images.ElementAt(image_num)) form_slide_test) |> ignore
        image_counter <- image_counter + 1)

button_next_image.Click.Add(fun _ ->
    image_counter <- image_counter + 1
    if (image_counter < images_count) then
        let image_num = random_list_for_images.[image_counter]
        (remove_image_in_form form_slide_test) |> ignore
        (show_image_in_form (new Point(640, 300)) 640 480 (images.ElementAt(image_num)) form_slide_test) |> ignore
    if (image_counter = images_count-1) then
        button_next_image.Enabled <- false
    if ((image_counter < images_count - 1) && (button_next_image.Enabled = false)) then
        button_next_image.Enabled <- true)

button_previous_image.Click.Add(fun _ ->
    image_counter <- image_counter - 1
    if (image_counter >= 0) then
        let image_num = random_list_for_images.[image_counter]
        (remove_image_in_form form_slide_test) |> ignore
        (show_image_in_form (new Point(640, 300)) 640 480 (images.ElementAt(image_num)) form_slide_test) |> ignore
    if (image_counter = 0) then
        button_previous_image.Enabled <- false
    if ((image_counter > 0) && (button_previous_image.Enabled = false)) then
        button_previous_image.Enabled <- true)



/// Add controls to form.
(form_control_adder form_slide_test [label_image; label_answer; label_correctness; label_correctness_answer; label_correctanswer; label_correctanswer_answer]) |> ignore
(form_control_adder form_slide_test [button_close; button_next_image; button_previous_image; button_answer_submit; button_reset_random_list; button_start]) |> ignore
(form_control_adder form_slide_test [textbox_answer]) |> ignore

//let counter_test = image_counter()
//let image_reader_test = (image_loader()).Count()

//label_correctanswer.Text <- counter_test.ToString()
//label_correctanswer_answer.Text <- image_reader_test.ToString()

form_slide_test.ShowDialog() |> ignore