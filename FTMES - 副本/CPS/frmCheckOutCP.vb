﻿Public Class frmCheckOutCP


    Private Sub frmCheckOutCP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim i As Integer

        '颜色/位置设定
        Me.BackColor = mydefaultcolor
        GroupBox4.BackColor = mydefaultcolor
        GroupBox5.BackColor = mydefaultcolor
        txt_cust.BackColor = mydefaultcolor
        txt_prod.BackColor = mydefaultcolor
        txt_ilot.BackColor = mydefaultcolor
        txt_clot.BackColor = mydefaultcolor
        txt_wqty.BackColor = mydefaultcolor
        txt_lastrecipeid.BackColor = mydefaultcolor
        txt_laststep.BackColor = mydefaultcolor
        txt_nextrecipeid.BackColor = mydefaultcolor
        txt_nextstep.BackColor = mydefaultcolor

        Me.Left = 10
        Me.Top = 10

        'Wafer选择表格行数设置
        DGV_WaferList.Visible = True

        For i = 0 To 24
            If mycheckio.waferlist.Substring(i, 1) = "1" Then
                DGV_WaferList(i, 0).Value = True
            End If
        Next

        '产品信息代入
        txt_cust.Text = mycheckio.custeng
        txt_prod.Text = mycheckio.iprod
        txt_ilot.Text = mycheckio.ilot
        txt_clot.Text = mycheckio.clot
        txt_wqty.Text = mycheckio.waferqty

        '站别信息带入
        txt_flowid.Text = mycheckio.flowid
        txt_currstep.Text = mycheckio.CurrentStep
        txt_currstatus.Text = mycheckio.status

        For i = 1 To UBound(mystep) - 1
            If mystep(i).name = mycheckio.CurrentStep Then '注意: 若同一个站别出现多次，会取最后一次出现的信息，所以此处并未用 exit for 跳出.
                txt_currrecipeid.Text = mystep(i).id

                If i > 1 Then
                    txt_laststep.Text = mystep(i - 1).name
                    txt_lastrecipeid.Text = mystep(i - 1).id
                End If

                If i < UBound(mystep) - 1 Then
                    txt_nextstep.Text = mystep(i + 1).name
                    txt_nextrecipeid.Text = mystep(i + 1).id
                End If

            End If
        Next
    End Sub


    Private Sub btn_confirm_Click(sender As Object, e As EventArgs) Handles btn_confirm.Click


        '插入LOTTRACK 记录表
        If SetLotTrackOUT(mycheckio.ilot, mycheckio.flowid, txt_currrecipeid.Text, mycheckio.CurrentStep, "", UserID) = False Then
            MsgBox("Check OUT 失败,插入LOTTRACK 记录表 FAIL,无法出站!", vbOKOnly + vbExclamation)
            Exit Sub
        Else
            ' do nothing
        End If

        '更新WIPLOT表 Status 为 下一站的CHECK IN
        If SetWIPLotStatus(mycheckio.ilot, txt_nextstep.Text, "CHECKIN") = False Then
            MsgBox("Check OUT 失败,更新WIPLOT表 Status 为 下一站CHECK IN 时 FAIL,无法出站!", vbOKOnly + vbExclamation)
            Exit Sub
        Else
            'do nothing
        End If

        Call frmCheckIO.btn_Query_Click("", e)

        MsgBox("Check Out 成功", vbOKOnly + vbInformation)
        Me.Close()


    End Sub

    Private Sub btn_exit_Click(sender As Object, e As EventArgs) Handles btn_exit.Click
        Me.Close()
    End Sub



End Class