using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CreateRoomDialogBase : UIDialogBase
{
    public CreateRoomDialogDetail detail;

    public override void SetAllMemberValue()
    {
        detail.CloseButton_Image = transform.Find("Root/CloseButton").gameObject.GetComponent<Image>();
        detail.CloseButton_Button = transform.Find("Root/CloseButton").gameObject.GetComponent<Button>();
        detail.CreateButton_Image = transform.Find("Root/CreateButton").gameObject.GetComponent<Image>();
        detail.CreateButton_Button = transform.Find("Root/CreateButton").gameObject.GetComponent<Button>();
        detail.PlayerNum2_CheckBoxSub = transform.Find("Root/PlayerNum2").gameObject.GetComponent<CheckBoxSub>();
        detail.PlayerNum4_CheckBoxSub = transform.Find("Root/PlayerNum4").gameObject.GetComponent<CheckBoxSub>();
        detail.Round4_CheckBoxSub = transform.Find("Root/Round4").gameObject.GetComponent<CheckBoxSub>();
        detail.Round8_CheckBoxSub = transform.Find("Root/Round8").gameObject.GetComponent<CheckBoxSub>();
        detail.Mode0_CheckBoxSub = transform.Find("Root/ModeGrid/Mode0").gameObject.GetComponent<CheckBoxSub>();
        detail.Mode1_CheckBoxSub = transform.Find("Root/ModeGrid/Mode1").gameObject.GetComponent<CheckBoxSub>();
        detail.Mode2_CheckBoxSub = transform.Find("Root/ModeGrid/Mode2").gameObject.GetComponent<CheckBoxSub>();
        detail.Mode3_CheckBoxSub = transform.Find("Root/ModeGrid/Mode3").gameObject.GetComponent<CheckBoxSub>();
        detail.Mode4_CheckBoxSub = transform.Find("Root/ModeGrid/Mode4").gameObject.GetComponent<CheckBoxSub>();
        detail.Mode5_CheckBoxSub = transform.Find("Root/ModeGrid/Mode5").gameObject.GetComponent<CheckBoxSub>();
        detail.Mode6_CheckBoxSub = transform.Find("Root/ModeGrid/Mode6").gameObject.GetComponent<CheckBoxSub>();
        detail.Mode7_CheckBoxSub = transform.Find("Root/ModeGrid/Mode7").gameObject.GetComponent<CheckBoxSub>();
        detail.ModeGrid_GridLayoutGroup = transform.Find("Root/ModeGrid").gameObject.GetComponent<GridLayoutGroup>();
        detail.Root_UtilScale = transform.Find("Root").gameObject.GetComponent<UtilScale>();

    }
}
