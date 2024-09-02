
// FontCreatorTestDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "FontCreatorTest.h"
#include "FontCreatorTestDlg.h"
#include "afxdialogex.h"

#include "BrainSystemsRunLenghFontDecoder.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CAboutDlg dialog used for App About

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ABOUTBOX };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

// Implementation
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(IDD_ABOUTBOX)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CFontCreatorTestDlg dialog



CFontCreatorTestDlg::CFontCreatorTestDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_FONTCREATORTEST_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_font_data = NULL;
}

CFontCreatorTestDlg::~CFontCreatorTestDlg()
{
	if (m_font_data != NULL)
	{
		delete m_font_data;
	}
}

void CFontCreatorTestDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_FONT_PIC, m_fontPic);
}

BEGIN_MESSAGE_MAP(CFontCreatorTestDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON1, &CFontCreatorTestDlg::OnBnClickedButton1)
END_MESSAGE_MAP()


// CFontCreatorTestDlg message handlers

BOOL CFontCreatorTestDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != nullptr)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here

	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CFontCreatorTestDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CFontCreatorTestDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CFontCreatorTestDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}




void CFontCreatorTestDlg::OnBnClickedButton1()
{
	// TODO: Add your control notification handler code here

	const TCHAR szFilter[] = _T("C header Files (*.h)|*.h||");
	CFileDialog dlg(TRUE, _T("*.h"), NULL, OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, szFilter, this);
	if (dlg.DoModal() == IDOK)
	{
		CStdioFile rFile;

		if (!rFile.Open(dlg.GetPathName(), CFile::modeRead))
		{
			MessageBox(_T("Can't OpenFile!"), _T("Warning"), MB_OK | MB_ICONHAND);
			return;
		}

		CString strHeaderFile;
		UINT nBytes = (UINT)rFile.GetLength();
		int nChars = nBytes / sizeof(TCHAR);
		nBytes = rFile.Read(strHeaderFile.GetBuffer(nChars), nBytes);
		strHeaderFile.ReleaseBuffer(nChars);


		UINT pos=strHeaderFile.Find("uint8_t");

		pos=strHeaderFile.Find(" ", pos);

		UINT pos2 = strHeaderFile.Find("[", pos+1);

		CString typeName = strHeaderFile.Mid(pos, pos2 - pos);

		UINT pos3 = strHeaderFile.Find("]", pos2 + 1);

		CString arraySizeStr = strHeaderFile.Mid(pos2+1, pos3 - pos2-1);

		int arraySize;
		sscanf_s(arraySizeStr.GetBuffer(), "%d", &arraySize);

		UINT end = pos3;


		if (m_font_data != NULL)
		{
			delete m_font_data;
		}

		m_font_data =new uint8_t[arraySize];


		int i = 0;
		while (1)
		{
			UINT start = strHeaderFile.Find("0x", end);
			if (start == -1)
			{
				break;
			}

			end = strHeaderFile.Find(",", start);

			CString valueStr = strHeaderFile.Mid(start, end);

			UINT value;
			sscanf_s(valueStr.GetBuffer(), "0x%X", &value);

			if (i<arraySize)
			{
				m_font_data[i++] = value;
			}

		}


		if (i != arraySize)
		{
			//error, data size mismatch
		}
		else
		{
			m_fontPic.SetFontData(m_font_data);
		}
	}
}
