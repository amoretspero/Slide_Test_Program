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
open System.Windows.Input

open Image_functions
open Text_functions

/// Fonts. - Must include : font name, font size, FontStyle, GraphicsUnit
let font_title = new Font("나눔손글씨 펜", 14.0f, FontStyle.Regular, GraphicsUnit.Point)
let font_text = new Font("맑은 고딕", 10.0f, FontStyle.Regular, GraphicsUnit.Point)
let font_label = new Font("서울남산체", 10.0f, FontStyle.Regular, GraphicsUnit.Point)

/// Forms for slide_test, test_selections. - Must include : Text, Width, Height, StartPosition, AutoScaleMode, FormBorderStyle
let form_slide_test = new Form(Width = 1280, Height = 720, StartPosition = FormStartPosition.CenterScreen, AutoScaleMode = AutoScaleMode.Font, FormBorderStyle = FormBorderStyle.FixedSingle, Text = "Slide Test")

/// Labels - Must include : Text, Width, Height, 
let label_image = new Label(Text = "Image", Width = 100, Height = 20, Location = new Point(600, 20), Font = font_label)
let label_answer = new Label(Text = "Answer :", Width = 70, Height = 20, Location = new Point(30, 650), Font = font_label)
let label_slide_name = new Label(Text = "이름 : ", Width = 60, Height = 20, Location = new Point(120, 650), Font = font_label)
let label_slide_founded_monument = new Label(Text = "출토 유적 : ", Width = 90, Height = 20, Location = new Point(320, 650), Font = font_label)
let label_slide_time = new Label(Text = "시기 : ", Width = 60, Height = 20, Location = new Point(550, 650), Font = font_label)
let label_slide_founded_place = new Label(Text = "출토 지역 : ", Width = 90, Height = 20, Location = new Point(750, 650), Font = font_label)
let label_slide_artist = new Label(Text = "작가 : ", Width = 60, Height = 20, Location = new Point(970, 650), Font = font_label)
let label_correctness = new Label(Text = "Correctness : ", Width = 100, Height = 20, Location = new Point(400, 600), Font = font_label)
let label_correctness_answer = new Label(Text = "N/A", Width = 100, Height = 20, Location = new Point(500, 600), Font = font_label)
let label_correctanswer = new Label(Text = "Correct Answer :", Width = 160, Height = 20, Location = new Point(480, 600), Font = font_label)
let label_correctanswer_answer = new Label(Text = "N/A", Width = 100, Height = 20, Location = new Point(650, 600), Font = font_text)
let label_show_answer_always = new Label(Text = "항상 정답 보이기", Width = 120, Height = 20, Location = new Point(1050, 100), Font = font_text)
let label_remaining_slide = new Label(Text = "현재 그림 번호 : ", Width = 120, Height = 20, Location = new Point(1025, 150), Font = font_text)
let label_remaining_slide_count = new Label(Text = "N/A", Width = 100, Height = 20, Location = new Point(1150, 150), Font = font_text)


/// Buttons - Must include : Text, Width, Height, Location, Font
let button_close = new Button(Text = "Close", Width = 70, Height = 25, Location = new Point(950, 25), Font = font_label)
let button_next_image = new Button(Text = "Next Image", Width = 120, Height = 25, Location = new Point(620, 550), Font = font_label, Enabled = false)
let button_previous_image = new Button(Text = "Prev Image", Width = 120, Height = 25, Location = new Point(480, 550), Font = font_label, Enabled = false)
let button_answer_submit = new Button(Text = "Submit", Width = 80, Height = 25, Location = new Point(1000, 600), Font = font_label, Enabled = false)
let button_reset_random_list = new Button(Text = "Random Order Reset", Width = 170, Height = 25, Location = new Point(1050, 25), Font = font_label)
let button_start = new Button(Text = "Test Start", Width = 120, Height = 25, Location = new Point(800, 25), Font = font_label)
let button_show_answer = new Button(Text = "정답 보기", Width = 120, Height = 25, Location = new Point(60, 540), Font = font_label, Enabled = false)
let button_hide_answer = new Button(Text = "정답 가리기", Width = 120, Height = 25, Location = new Point(60, 570), Font = font_label, Enabled = false)
let button_unset_random = new Button(Text = "Unset Random", Width = 120, Height = 20, Location = new Point(1050, 60), Font = font_text)

/// Text boxes - Must include : Text, Width, Height, Location, Font
let textbox_answer_name = new TextBox(Text = "", Width = 120, Height = 25, Location = new Point(180, 645), Font = font_text, Enabled = false)
let textbox_answer_founded_monument = new TextBox(Text = "", Width = 120, Height = 25, Location = new Point(410, 645), Font = font_text, Enabled = false)
let textbox_answer_time = new TextBox(Text = "", Width = 120, Height = 25, Location = new Point(610, 645), Font = font_text, Enabled = false)
let textbox_answer_founded_place = new TextBox(Text = "", Width = 120, Height = 25, Location = new Point(840, 645), Font = font_text, Enabled = false)
let textbox_answer_artist = new TextBox(Text = "", Width = 120, Height = 25, Location = new Point(1030, 645), Font = font_text, Enabled = false)
let textbox_answer_total = new RichTextBox(Text = "", Width = 240, Height = 480, Location = new Point(20, 40), Font = font_text, BorderStyle = BorderStyle.FixedSingle)

let checkbox_show_answer_always = new CheckBox(Location = new Point(1175, 100))

let mutable unset_random = false


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
let random_list_for_images = ref (random_list_gen images_count [])

/// Image counter
let mutable image_counter = 0
let mutable image_number = 0

/// Get answers in form of list.
let answers = text_to_list()

/// Event handlers.
button_close.Click.Add(fun _ ->
    form_slide_test.Close())

button_start.Click.Add(fun _ ->
    textbox_answer_total.Text <- ""
    if (image_counter = 0) then
        let image_num = random_list_for_images.Value.[0]
        (show_image_in_form (new Point(640, 300)) 640 480 (images.ElementAt(image_num)) form_slide_test) |> ignore
        button_next_image.Enabled <- true
        button_previous_image.Enabled <- true
        button_previous_image.PerformClick()
        button_show_answer.Enabled <- true
        button_hide_answer.Enabled <- true
        image_number <- image_num
    else
        image_counter <- 0
        button_start.PerformClick()
    if (checkbox_show_answer_always.Checked) then
        button_show_answer.PerformClick()
    label_remaining_slide_count.Text <- (image_counter+1).ToString()+" / "+(images_count).ToString())

button_next_image.Click.Add(fun _ ->
    image_counter <- image_counter + 1
    textbox_answer_total.Text <- ""
    if (image_counter < images_count) then
        let image_num = random_list_for_images.Value.[image_counter]
        (remove_image_in_form form_slide_test) |> ignore
        (show_image_in_form (new Point(640, 300)) 640 480 (images.ElementAt(image_num)) form_slide_test) |> ignore
        image_number <- image_num
    if (image_counter = images_count-1) then
        button_next_image.Enabled <- false
    if ((image_counter < images_count - 1) && (button_next_image.Enabled = false)) then
        button_next_image.Enabled <- true
    if (image_counter = 1) then
        button_previous_image.Enabled <- true
    if (checkbox_show_answer_always.Checked) then
        button_show_answer.PerformClick()
    label_remaining_slide_count.Text <- (image_counter+1).ToString()+" / "+(images_count).ToString())

button_previous_image.Click.Add(fun _ ->
    image_counter <- image_counter - 1
    textbox_answer_total.Text <- ""
    if (image_counter >= 0) then
        let image_num = random_list_for_images.Value.[image_counter]
        (remove_image_in_form form_slide_test) |> ignore
        (show_image_in_form (new Point(640, 300)) 640 480 (images.ElementAt(image_num)) form_slide_test) |> ignore
        image_number <- image_num
    else
        image_counter <- 0
    if (image_counter = 0) then
        button_previous_image.Enabled <- false
    if ((image_counter > 0) && (button_previous_image.Enabled = false)) then
        button_previous_image.Enabled <- true
    if (image_counter = images_count - 2) then
        button_next_image.Enabled <- true
    if (checkbox_show_answer_always.Checked) then
        button_show_answer.PerformClick()
    label_remaining_slide_count.Text <- (image_counter+1).ToString()+" / "+(images_count).ToString())

button_reset_random_list.Click.Add(fun _ ->
    textbox_answer_total.Text <- ""
    random_list_for_images.Value <- (random_list_gen images_count [])
    (remove_image_in_form form_slide_test) |> ignore
    image_counter <- 0
    button_next_image.Enabled <- false
    button_previous_image.Enabled <- false
    button_show_answer.Enabled <- false
    button_hide_answer.Enabled <- false
    label_remaining_slide_count.Text <- "N/A")

button_unset_random.Click.Add(fun _ ->
    textbox_answer_total.Text <- ""
    random_list_for_images.Value <- [0 .. (images_count-1)]
    (remove_image_in_form form_slide_test) |> ignore
    image_counter <- 0
    button_next_image.Enabled <- false
    button_previous_image.Enabled <- false
    button_show_answer.Enabled <- false
    button_hide_answer.Enabled <- false
    label_remaining_slide_count.Text <- "N/A")

button_show_answer.Click.Add(fun _ ->
    if (image_counter >= answers.Length) then
        textbox_answer_total.Text <- "정답의 갯수와 이미지의 갯수가 일치하지 않습니다."
    else
        textbox_answer_total.Text <- answers.[image_number])

button_hide_answer.Click.Add(fun _ ->
    textbox_answer_total.Text <- "")

form_slide_test.KeyDown.Add(fun arg ->
    if (arg.KeyCode = System.Windows.Forms.Keys.Enter) then
        button_next_image.PerformClick()
    if (arg.KeyCode = System.Windows.Forms.Keys.Back) then
        button_previous_image.PerformClick())



/// Add controls to form.
(form_control_adder form_slide_test [label_image; (*label_answer; label_slide_name; label_slide_founded_monument; label_slide_time; label_slide_founded_place; label_slide_artist;*)
                                    (*label_correctness; label_correctness_answer; label_correctanswer; label_correctanswer_answer;*) label_show_answer_always;
                                    label_remaining_slide; label_remaining_slide_count]) |> ignore
(form_control_adder form_slide_test [button_close; button_next_image; button_previous_image; button_answer_submit; button_reset_random_list; button_start; button_show_answer; button_hide_answer;
                                    button_unset_random]) |> ignore
(form_control_adder form_slide_test [(*textbox_answer_name; textbox_answer_founded_monument; textbox_answer_time; textbox_answer_founded_place; textbox_answer_artist;*) textbox_answer_total]) |> ignore
(form_control_adder form_slide_test [checkbox_show_answer_always]) |> ignore

//let counter_test = image_counter()
//let image_reader_test = (image_loader()).Count()

//label_correctanswer.Text <- counter_test.ToString()
//label_correctanswer_answer.Text <- image_reader_test.ToString()

form_slide_test.ShowDialog() |> ignore