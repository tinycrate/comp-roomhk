using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionViewController : MonoBehaviour, IMainGameView {

    public Animator Animator => GetComponent<Animator>();
    public GameObject CurrentGameObject => gameObject;
    
    public GameObject DownTimeItem;
    public GameObject DownTimeItemContainer;
    public ScrollRect DownTimeScrollRect;

    // Start is called before the first frame update
    void Start() {
        foreach (var outage in GameManager.GetInstance.StatManager.Outages) {
            CreateOutageItem(outage);
        }
        GameManager.GetInstance.ProductionService.OnProductionDowntime += OnProductionDowntime;
    }

    void OnDestroy() {
        GameManager.GetInstance.ProductionService.OnProductionDowntime -= OnProductionDowntime;
    }

    private void OnProductionDowntime(object sender, Outage e) {
        CreateOutageItem(e);
    }

    private void CreateOutageItem(Outage outage) {
        var newObject = Instantiate(DownTimeItem, DownTimeItemContainer.transform);
        newObject.GetComponent<Text>().text =
            $"Day {outage.ReportDate}: {outage.OutageName} ({outage.TimeToRepair * 24:0.##} hours)";
        newObject.SetActive(true);
        DownTimeScrollRect.normalizedPosition = new Vector2(0, 0);
    }

}
