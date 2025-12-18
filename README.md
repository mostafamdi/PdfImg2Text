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

---

## Final Setup

1. Install **ImageMagick**
2. Install **Tesseract OCR**
3. Copy the required `.traineddata` language files into the `tessdata` folder
4. Install **PDF-IMG2Txt**
5. In the application settings, set the **Tesseract installation directory path**

After completing these steps, the application will be ready to use.
