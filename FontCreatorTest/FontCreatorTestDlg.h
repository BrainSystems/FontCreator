
// FontCreatorTestDlg.h : header file
//

#pragma once

#include "FontPictureControl.h"

// CFontCreatorTestDlg dialog
class CFontCreatorTestDlg : public CDialogEx
{
// Construction
public:
	CFontCreatorTestDlg(CWnd* pParent = nullptr);	// standard constructor
	~CFontCreatorTestDlg();

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_FONTCREATORTEST_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;
	uint8_t* m_font_data;
	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButton1();
	CFontPictureControl m_fontPic;
};
