## Prerequisites

This application requires the following dependencies to be installed before use:

### 1. ImageMagick
ImageMagick must be installed on your system.

- Recommended: 64-bit version
- Official download page:  
  https://imagemagick.org/script/download.php

---

### 2. Tesseract OCR
Tesseract OCR is required for text recognition.

#### Download Tesseract OCR:
https://sourceforge.net/projects/tesseract-ocr.mirror/

#### Download language files:
https://github.com/tesseract-ocr/tessdata

- Language files have the `.traineddata` extension.
- After downloading, copy the required language files into the `tessdata` directory inside your Tesseract installation path.
- Add the Tesseract installation directory to your system PATH environment variable.
- <img width="600" height="666" alt="enviroment variable" src="https://github.com/user-attachments/assets/470da224-37ac-45d3-91b6-c07a2d0924d6" />

---

## Final Setup

1. Install **ImageMagick**
2. Install **Tesseract OCR**
3. Copy the required `.traineddata` language files into the `tessdata` folder
4. Install **PDF-IMG2Txt**
5. In the application settings, set the **Tesseract installation directory path**
<img width="497" height="362" alt="Screenshot 2025-12-25 161921" src="https://github.com/user-attachments/assets/91359bf4-cefb-4d13-9c3e-b287fa1c4d02" />

After completing these steps, the application will be ready to use.

<img width="510" height="377" alt="PDF-IMG2Txt" src="https://github.com/user-attachments/assets/0033b5f9-bbbc-40a9-ad6e-147e511e99f8" />

