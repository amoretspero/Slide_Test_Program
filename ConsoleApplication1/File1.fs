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
open List_functions

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
let label_show_help = new Label(Text = "도움말 보이기", Width = 120, Height = 20, Location = new Point(1050, 122), Font = font_text)
let label_remaining_slide = new Label(Text = "현재 그림 번호 : ", Width = 120, Height = 20, Location = new Point(1025, 150), Font = font_text)
let label_remaining_slide_count = new Label(Text = "N/A", Width = 100, Height = 20, Location = new Point(1150, 150), Font = font_text)


/// Buttons - Must include : Text, Width, Height, Location, Font
let button_close = new Button(Text = "Close", Width = 70, Height = 25, Location = new Point(950, 15), Font = font_label)
let button_next_image = new Button(Text = "Next Image", Width = 120, Height = 25, Location = new Point(520, 550), Font = font_label, Enabled = false)
let button_previous_image = new Button(Text = "Prev Image", Width = 120, Height = 25, Location = new Point(380, 550), Font = font_label, Enabled = false)
let button_answer_submit = new Button(Text = "Submit", Width = 80, Height = 25, Location = new Point(1000, 600), Font = font_label, Enabled = false)
let button_reset_random_list = new Button(Text = "Random Order Reset", Width = 170, Height = 25, Location = new Point(1050, 15), Font = font_label)
let button_start = new Button(Text = "Test Start", Width = 120, Height = 25, Location = new Point(800, 15), Font = font_label)
let button_show_answer = new Button(Text = "정답 보기", Width = 120, Height = 25, Location = new Point(60, 600), Font = font_label, Enabled = false)
let button_hide_answer = new Button(Text = "정답 가리기", Width = 120, Height = 25, Location = new Point(60, 630), Font = font_label, Enabled = false)
let button_unset_random = new Button(Text = "Unset Random", Width = 120, Height = 20, Location = new Point(1050, 50), Font = font_text)

/// Text boxes - Must include : Text, Width, Height, Location, Font
let textbox_answer_name = new TextBox(Text = "", Width = 120, Height = 25, Location = new Point(180, 645), Font = font_text, Enabled = false)
let textbox_answer_founded_monument = new TextBox(Text = "", Width = 120, Height = 25, Location = new Point(410, 645), Font = font_text, Enabled = false)
let textbox_answer_time = new TextBox(Text = "", Width = 120, Height = 25, Location = new Point(610, 645), Font = font_text, Enabled = false)
let textbox_answer_founded_place = new TextBox(Text = "", Width = 120, Height = 25, Location = new Point(840, 645), Font = font_text, Enabled = false)
let textbox_answer_artist = new TextBox(Text = "", Width = 120, Height = 25, Location = new Point(1030, 645), Font = font_text, Enabled = false)
let textbox_answer_total = new RichTextBox(Text = "", Width = 720, Height = 60, Location = new Point(220, 600), Font = font_text, BorderStyle = BorderStyle.FixedSingle, Enabled = true)

/// Check boxes - Must include : Location
let checkbox_show_answer_always = new CheckBox(Location = new Point(1175, 100))
let checkbox_show_help = new CheckBox(Location = new Point(1175, 120), Checked = true)

/// Help - 1. Key operations, 2. Random list Gen, 3. Mail to
let textbox_help = new RichTextBox(Text = "", Width = 240, Height = 480, Location = new Point(1000, 180), Font = font_text, BorderStyle = BorderStyle.None, Enabled = true)

/// Represents whether random list is enabled or disabled.
let mutable unset_random = false

/// Sets help message.
let help_message1 = "1.키 조작 방법\n"+"Ctrl + S : 테스트 시작\n"+"Shift + Esc : 프로그램 종료\n"+"Ctrl + (-> or L) : 다음 이미지\n"+"Ctrl + (<- or J) : 이전 이미지\n"+"Ctrl + I : 정답 보기\n"+"Ctrl + J : 정답 가리기\n"+"Ctrl + R : 순서 다시 섞기\n"+"Ctrl + U : 순서 섞기 해제\n\n"
let help_message2 = "2. 순서 섞기\n"+"순서는 프로그램 실행 시\n임의로 생성됩니다.\n"+"다시 섞고 싶다면\n'Random Order Reset' 버튼을,\n"+"순서를 섞고 싶지 않다면\n'Unset Random' 버튼을 클릭합니다.\n\n"
let help_message3 = "3. 버그 신고, 개선사항 건의\n"+"amoretspero@snu.ac.kr"
let help_message = help_message1+help_message2+help_message3
textbox_help.Text <- help_message

/// Form control adder.
let rec form_control_adder (f : Form) (lst : Control list) =
    match lst with
    [] -> f
    | h :: t -> 
        f.Controls.Add(h)
        (form_control_adder f t)

/// Form control remover.
let rec form_control_remover (f : Form) (lst : Control list) =
    match lst with
    [] -> f
    | h :: t ->
        f.Controls.Remove(h)
        (form_control_remover f t)

/// Imaged loaded from specified folder. Only finds images with format JPEG(.jpg)
//let images = image_loader()
//let images_count = image_counter()
let plural_images = get_plural_images()
let plural_images_count = plural_images.Length

/// Generate random list for images.
//let random_list_for_images = ref (random_list_gen images_count [] [0 .. (images_count - 1)])
let plural_random_list_for_images = ref (random_list_gen plural_images_count [] [0 .. (plural_images_count - 1)])

/// Image counter
let mutable image_counter = 0
let mutable image_number = 0

/// Get answers in form of list.
let answers = text_to_list()

/// Event handlers.
button_close.Click.Add(fun _ ->
    form_slide_test.Close())

/// Setting KeyPreview option of form to catch key press events before they are sent to focused controls.
form_slide_test.KeyPreview <- true

/// Key pressing combinations. Ctrl + (-> or L) : next image, Ctrl + (<- or J) : prev image, Ctrl + S : Start, Shift + Esc : Close, Ctrl + I : Show answer, Ctrl + J : Hide answer.
form_slide_test.KeyDown.Add(fun e ->
    if e.Control = true && (e.KeyCode = Keys.Right || e.KeyCode = Keys.L) then 
        System.Console.Write("Ctrl + ->(right arrow) Key(s) are pressed!\n")
        button_next_image.PerformClick()
    if e.Control = true && (e.KeyCode = Keys.Left || e.KeyCode = Keys.J) then
        System.Console.Write("Ctrl + <-(left arrow) Key(s) are pressed!\n")
        button_previous_image.PerformClick()
    if e.Control = true && e.KeyCode = Keys.S then
        System.Console.Write("Ctrl + S Key(s) are pressed!\n")
        button_start.PerformClick()
    if e.Shift = true && e.KeyCode = Keys.Escape then
        System.Console.Write("Shift + Esc Key(s) are pressed!\n")
        button_close.PerformClick()
    if e.Control = true && e.KeyCode = Keys.I then
        System.Console.Write ("Ctrl + Enter Key(s) are pressed!\n")
        button_show_answer.PerformClick()
    if e.Control = true && e.KeyCode = Keys.K then
        System.Console.Write ("Ctrl + Backspace Key(s) are pressed!\n")
        button_hide_answer.PerformClick()
    if e.Control = true && e.KeyCode = Keys.R then
        System.Console.Write ("Ctrl + R Key(s) are pressed!\n")
        button_reset_random_list.PerformClick()
    if e.Control = true && e.KeyCode = Keys.U then
        System.Console.Write ("Ctrl + U Key(s) are pressed!\n")
        button_unset_random.PerformClick()
        )

        

button_start.Click.Add(fun _ ->
    textbox_answer_total.Text <- ""
    if (image_counter = 0) then
        //let image_num = random_list_for_images.Value.[0]
        let image_num = plural_random_list_for_images.Value.[0]
        let plural_info = plural_images.[image_num]
        let real_image_num = plural_info.[0]
        let real_image_cnt = plural_info.[1]
        let images = get_images real_image_num real_image_cnt
        //(show_image_in_form (new Point(640, 300)) 640 480 (images.ElementAt(image_num)) form_slide_test) |> ignore
        show_image_in_form_plural_gen real_image_cnt images form_slide_test
        button_next_image.Enabled <- true
        button_previous_image.Enabled <- true
        button_previous_image.PerformClick()
        button_show_answer.Enabled <- true
        button_hide_answer.Enabled <- true
        image_number <- image_num
    else
        remove_image_in_form form_slide_test |> ignore
        image_counter <- 0
        button_start.PerformClick()
    if (checkbox_show_answer_always.Checked) then
        button_show_answer.PerformClick()
    label_remaining_slide_count.Text <- (image_counter+1).ToString()+" / "+(plural_images_count).ToString())

button_next_image.Click.Add(fun _ ->
    image_counter <- image_counter + 1
    textbox_answer_total.Text <- ""
    if (image_counter < plural_images_count) then
        //let image_num = random_list_for_images.Value.[image_counter]
        let image_num = plural_random_list_for_images.Value.[image_counter]
        let plural_info = plural_images.[image_num]
        let real_image_num = plural_info.[0]
        let real_image_cnt = plural_info.[1]
        let images = get_images real_image_num real_image_cnt
        (remove_image_in_form form_slide_test) |> ignore
        //(show_image_in_form (new Point(640, 300)) 640 480 (images.ElementAt(image_num)) form_slide_test) |> ignore
        show_image_in_form_plural_gen real_image_cnt images form_slide_test
        image_number <- image_num
    if (image_counter = plural_images_count-1) then
        button_next_image.Enabled <- false
    if ((image_counter < plural_images_count - 1) && (button_next_image.Enabled = false)) then
        button_next_image.Enabled <- true
    if (image_counter = 1) then
        button_previous_image.Enabled <- true
    if (checkbox_show_answer_always.Checked) then
        button_show_answer.PerformClick()
    label_remaining_slide_count.Text <- (image_counter+1).ToString()+" / "+(plural_images_count).ToString())

button_previous_image.Click.Add(fun _ ->
    image_counter <- image_counter - 1
    textbox_answer_total.Text <- ""
    if (image_counter >= 0) then
        //let image_num = random_list_for_images.Value.[image_counter]
        let image_num = plural_random_list_for_images.Value.[image_counter]
        let plural_info = plural_images.[image_num]
        let real_image_num = plural_info.[0]
        let real_image_cnt = plural_info.[1]
        let images = get_images real_image_num real_image_cnt
        (remove_image_in_form form_slide_test) |> ignore
        //(show_image_in_form (new Point(640, 300)) 640 480 (images.ElementAt(image_num)) form_slide_test) |> ignore
        show_image_in_form_plural_gen real_image_cnt images form_slide_test
        image_number <- image_num
    else
        image_counter <- 0
    if (image_counter = 0) then
        button_previous_image.Enabled <- false
    if ((image_counter > 0) && (button_previous_image.Enabled = false)) then
        button_previous_image.Enabled <- true
    if (image_counter = plural_images_count - 2) then
        button_next_image.Enabled <- true
    if (checkbox_show_answer_always.Checked) then
        button_show_answer.PerformClick()
    label_remaining_slide_count.Text <- (image_counter+1).ToString()+" / "+(plural_images_count).ToString())

button_reset_random_list.Click.Add(fun _ ->
    textbox_answer_total.Text <- ""
    //random_list_for_images.Value <- (random_list_gen images_count [] [0 .. (images_count - 1)])
    plural_random_list_for_images.Value <- (random_list_gen plural_images_count [] [0 .. (plural_images_count - 1)])
    (remove_image_in_form form_slide_test) |> ignore
    image_counter <- 0
    button_next_image.Enabled <- false
    button_previous_image.Enabled <- false
    button_show_answer.Enabled <- false
    button_hide_answer.Enabled <- false
    label_remaining_slide_count.Text <- "N/A")

button_unset_random.Click.Add(fun _ ->
    textbox_answer_total.Text <- ""
    //random_list_for_images.Value <- [0 .. (images_count-1)]
    plural_random_list_for_images.Value <- [0 .. (plural_images_count-1)]
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

checkbox_show_help.Click.Add(fun _ ->
    if (checkbox_show_help.Checked = false) then
        (form_control_remover form_slide_test [textbox_help]) |> ignore
    else
        (form_control_adder form_slide_test [textbox_help]) |> ignore
    )

form_slide_test.KeyDown.Add(fun arg ->
    if (arg.KeyCode = System.Windows.Forms.Keys.Enter) then
        button_next_image.PerformClick()
    if (arg.KeyCode = System.Windows.Forms.Keys.Back) then
        button_previous_image.PerformClick())



/// Add controls to form.
(form_control_adder form_slide_test [(*label_image;*) label_show_answer_always; label_show_help;
                                    label_remaining_slide; label_remaining_slide_count]) |> ignore
(form_control_adder form_slide_test [button_close; button_next_image; button_previous_image; button_reset_random_list; button_start; button_show_answer; button_hide_answer;
                                    button_unset_random]) |> ignore
(form_control_adder form_slide_test [textbox_answer_total; textbox_help]) |> ignore
(form_control_adder form_slide_test [checkbox_show_answer_always; checkbox_show_help]) |> ignore

//let counter_test = image_counter()
//let image_reader_test = (image_loader()).Count()

//label_correctanswer.Text <- counter_test.ToString()
//label_correctanswer_answer.Text <- image_reader_test.ToString()

form_slide_test.ShowDialog() |> ignore