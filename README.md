# R1 DIY Robot Kit

![Assembled R1 DIY Robot](./images/main_robot_photo.png)
*Assembled R1 DIY Robot with face projection, ready for interaction.*

Welcome to the **R1 DIY Kit**! These files contain every step needed to manufacture and build your very own R1 robot. This DIY kit is based on the original R1 design, with the same internal design and hardware. 

While most instructions are provided in video format, important details can also be found in the accompanying README PDFs throughout the repository. Please read the provided documentation carefully to ensure successful assembly and setup.

## Table of Contents

1. [Gather Required Parts](#gather-required-parts)
2. [3D Printing Files](#3d-printing-files)
3. [Build Instructions](#build-instructions)
4. [Software Setup](#software-setup)
   - [Pre-Setup](#pre-setup)
   - [Robot Computer Setup](#robot-computer-setup)
   - [Installing Unity](#installing-unity)
   - [Starter Mode for Mac](#starter-mode-for-mac)
   - [Starter Mode for Windows](#starter-mode-for-windows)
   - [Face Adjustment](#face-adjustment)
   - [Pausing the Robot](#pausing-the-robot)
   - [Custom Faces](#custom-faces)
   - [Arduino Setup](#arduino-setup)
   - [Advanced Mode](#advanced-mode)
5. [Unity Project Files](#unity-project-files)
6. [Connecting to a Local LLM](#connecting-to-a-local-llm)
7. [Connecting to Google Gemini API](#connecting-to-google-gemini-api)

---

## 1. Gather Required Parts

Before you begin building, print the files and gather all the necessary parts. Here is the bill of materials with links to the components you will need to purchase:

- [Parts List README (PDF)](./pre-setup/R1_Parts_List_README.pdf)

---

## 2. 3D Printing Files

The robot's frame is 3D printed. All the necessary STL files and instructions for printing are included in the following folder. Please refer to the "Printing Guide PDF" for important details about printing the pieces:

- [STL Files for 3D Printing](./build-instructions/Printing_Files)
- [Printing Guide (PDF)](./build-instructions/R1_Printing_Readme.pdf)

---

## 3. Build Instructions

Once you have printed all the parts, it's time to assemble the robot. Follow the video link and refer to the provided assembly guide for detailed instructions:

- [Assembly Instructions PDF](./build-instructions/Instruction_README.pdf)
- [Build Instruction Video](https://youtu.be/2tXyRC_KmT0)

---

## 4. Software Setup

After building the robot, the next step is setting up the software. Most of the instructions are in video format, and the following index outlines the sequence to follow for setting everything up.

### Pre-Setup

Start by setting up the required API keys. You will need both an OpenAI API Key and an Azure Key.

- [Azure Key Setup Video](https://youtu.be/wxKWgDN9a2c)
- [OpenAI Key Setup Video](https://youtu.be/aMjvTZvdCsI)

---

### Robot Computer Setup

Next, set up the robot's computer to run the required software. Follow the instructions in the video below:

- [Computer Setup Video](https://youtu.be/bzLXkPkEXfA)

---

### Installing Unity

You will need to install a specific version of Unity to configure the robot. This guide covers installation for both Mac and Windows users:

- [Unity Installation Video (Cross-platform)](https://youtu.be/6DMLwrmqWGs)

---

### Starter Mode for Mac

Mac users should follow the instructions in the video below to set up Starter Mode for the robot:

- [Starter Mode Mac Video](https://youtu.be/YG3O_CxVHSQ)

---

### Starter Mode for Windows

Alternatively, Windows users can follow this video for setting up Starter Mode:

- [Starter Mode Windows Video](https://youtu.be/5EzZFH6TbZU)

---

### Face Adjustment

Once the software is set up, you may want to adjust the robot's projected face. This video provides a comprehensive overview of how to make these adjustments. Make sure to read the accompanying PDF for important projector notes:

- [Face Adjustment Video](https://youtu.be/Ks0d70vO0KM)
- [Projector README (PDF)](./face-adjustment/Projector_README.pdf)

---

### Pausing the Robot

To learn how to pause and resume the robot while it is running, refer to the following video:

- [Pausing the Robot Video](https://youtu.be/hnXKXJgQXd8)

---

### Custom Faces

You can add custom faces to the robot. Instructions are available for both Mac and Windows users. Additionally, the face template images referenced in the instructional videos are included:

- [Custom Faces for Mac Video](https://youtu.be/4W-XHF8ed94)
- [Custom Faces for Windows Video](https://youtu.be/J4PsrUzNvfo)
- [Face Template Images](./custom-faces/Face_Template_Images)

---

### Arduino Setup

The Arduino controls the head movement of the robot. Use the following instructions to set it up, and make sure to use the provided circuit diagram and Arduino sketches:

- [Arduino Setup Video](https://youtu.be/LZN1rNx5XJ8)
- [Circuit Diagram (PNG)](./arduino-setup/Circuit_Diagram.png)
- [Simple Sketch (Arduino Script)](./arduino-setup/Simple_Sketch.ino)
- [Advanced Sketch (Arduino Script)](./arduino-setup/Advanced_Sketch.ino)

---

### Advanced Mode

For more advanced functionality, such as modifying the robot's mouth sprites and other detailed features, follow the instructions in the advanced video below:

- [Advanced Mode Video](https://youtu.be/tfWZQt3Dw8M)

---

## 5. Unity Project Files

The R1 robot has two versions of the Unity project, each with different mouth sprites. The **r1-starter-mode-f** contains female mouth sprites, while **r1-starter-mode-m** contains male mouth sprites. You can choose which one to use based on your preference:

- [Starter Mode Female Project (Unity)](./r1-starter-mode-f)
- [Starter Mode Male Project (Unity)](./r1-starter-mode-m)

---

## 6. Connecting to a Local LLM

The R1 robot can be powered by a locally hosted language model, providing real-time conversational abilities. These instructions cover setting up the robot to work with [Oobabooga's text-gen-webui](https://github.com/oobabooga/text-generation-webui) as the backend for a Local LLM.

This setup allows the robot to connect to and be powered by a locally hosted AI model of your choosing.

### Step-by-Step Instructions

1. **Setting Up the Unity Project**:
   - Navigate to the `LocalLLM` folder, which contains two scripts: `RoboLogic.cs` and `RoboListen.cs`.
   - In your Unity project, open the existing `RoboLogic.cs` and `RoboListen.cs` scripts.
   - Replace the contents of these scripts with the versions from the `LocalLLM` folder. This will configure the robot to communicate with the local LLM setup.

2. **Starting the Web UI with the Correct Flags**:
   - Launch the web UI with the `--listen` flag to allow external connections.
   - In the web UI, go to the **Session** tab and select the **OpenAI** extension. Click **Apply** and restart the session.

   ![Session Tab - OpenAI Extension](./images/Image1.png)

3. **Load Your Chosen Model**:
   - Load the AI model you wish to use as you normally would within the web UI.
   - Ensure that the machine running the web UI is configured to allow communication on **port 5000**, which is required for the robot to connect.

   For more detailed information on setting up the OpenAI API within the text-gen-webui, refer to [this guide](https://github.com/oobabooga/text-generation-webui/wiki/12-%E2%80%90-OpenAI-API).

4. **Configuring Unity for Local LLM Connection**:
   - In Unity’s **Inspector** pane, enter the name of the character you wish to communicate with in the web UI, as well as the endpoint URL for the web UI machine.

   ![Local LLM Connection Settings in Unity Inspector](./images/Image3.png)

   - To find the character name in the web UI, go to **Parameters > Chat > Character**.

   ![Character Selection in Web UI](./images/Image4.png)

5. **Testing the Connection**:
   - If everything is configured correctly, you should see the LLM processing requests from the robot in the terminal where the web UI is running.

   ![Terminal Output on LLM Machine](./images/Image5.png)

---

## 7. Connecting to Google Gemini API

The R1 robot can also be configured to use Google's Gemini API for its conversational abilities, as an alternative to a local LLM or other services. This setup uses the `RoboLogic.cs` script which has been adapted for the Gemini API.

### Prerequisites:

*   **Google Cloud Project:** You need a Google Cloud Project set up.
*   **Gemini API Enabled:** Ensure the "Generative Language API" (or "Vertex AI API" if accessing Gemini through Vertex) is enabled for your project.
*   **API Key:** You must have an API key associated with your project that is authorized to use the Gemini API.

### Step-by-Step Instructions:

1.  **Obtain your Google API Key:**
    *   Go to the [Google Cloud Console](https://console.cloud.google.com/).
    *   Navigate to your project.
    *   Go to "APIs & Services" > "Credentials".
    *   Create a new API key if you don't have one, or use an existing key. Ensure it's properly restricted for security, but allows access to the Generative Language API.
    *   **Important:** Keep this API key secure. Do not share it publicly or commit it directly into version control if you are managing your project on a public repository.

2.  **Configure Unity Project:**
    *   Open your R1 Unity project (e.g., `r1-starter-mode-f` or `r1-starter-mode-m`).
    *   Locate the GameObject that has the `RoboLogic.cs` script attached to it (this is typically the main robot controller object).
    *   In the Unity Inspector window for this GameObject, you will find the following fields under the "Robo Logic" component:
        *   **Gemini Api Key:** Paste your Google API Key here.
        *   **Gemini Model Name:** Enter the specific Gemini model you wish to use (e.g., `gemini-1.5-flash-latest`, `gemini-pro`, `gemini-1.0-pro`). The default is often `gemini-1.5-flash-latest`. You can find available model names in the [Google AI documentation](https://ai.google.dev/models/gemini).

    ![Gemini API Connection Settings in Unity Inspector](./images/Gemini_Inspector_Setup.png) 
    *(Note: You will need to add an illustrative image named `Gemini_Inspector_Setup.png` to the `images/` directory later if you want this image to show. For now, the markdown link is a placeholder.)*

3.  **Ensure Correct Scripts:**
    *   Make sure the `LocalLLM/RoboLogic.cs` script in your Unity project is the version that has been modified for Gemini API access. If you downloaded the project after these changes, it should be correct. If you are modifying an older version, ensure you've updated the script.

4.  **Testing the Connection:**
    *   Run the Unity scene.
    *   When the robot attempts to get a response (e.g., after speech-to-text processing via `RoboListen`), it will now call the Google Gemini API.
    *   Check the Unity Console for any log messages from `RoboLogic.cs`. Successful calls will log the received response, while errors will detail issues with the API key, model name, endpoint, or network connectivity.

### Important Considerations:

*   **API Costs:** Using the Google Gemini API may incur costs based on your usage. Familiarize yourself with the [Gemini API pricing](https://ai.google.dev/pricing).
*   **Rate Limits:** Be aware of any rate limits imposed by the API to avoid service interruptions.
*   **Security:** Handle your API key with care. For more advanced or production scenarios, consider more secure ways to manage API keys rather than pasting them directly into the Inspector, such as using environment variables or a secure configuration service (though this is beyond the scope of the current basic setup).

---

If you have any questions, feel free to reach out to me. Enjoy building your R1 robot, and remember to follow all safety guidelines when working with tools and equipment!