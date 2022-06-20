using System;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{
    [SerializeField] private Image potionImage;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button submitButton;
    [SerializeField] private Button[] materialButtons;

    private const int TotalLimit = 10000; // 최대 재료 추가 횟수
    private const int MaterialLimit = 5; // 재료당 최대 추가 횟수
    private const float ColorRatio = 1f / MaterialLimit; // 횟수당 더해지는 색상 수치

    private int totalCount; // 현재 재료 추가 횟수
    private readonly int[] counts = new int[Enum.GetValues(typeof(Material)).Length]; // 재료당 현재 추가 횟수

    private void Start()
    {
        ResetPotionMaterial();
        resetButton.onClick.AddListener(ResetPotionMaterial);
        submitButton.onClick.AddListener(MakePotion);

        for (var i = 0; i < materialButtons.Length; i++)
        {
            var arg = (Material)(i % 3);
            var plus = i % 2 == 0;
            materialButtons[i].onClick.AddListener(() => AddPotionMaterial(arg, plus));
        }
    }

    // 조합 데이터 초기화
    private void ResetPotionMaterial()
    {
        totalCount = 0;
        for (var i = 0; i < counts.Length; i++)
        {
            counts[i] = 0;
        }

        ShowPotionColor();
    }

    // 조합
    private void AddPotionMaterial(Material potionMaterial, bool plus)
    {
        if (TotalLimit <= totalCount) return; // 전체 조합 물약 개수 제한

        var index = (int)potionMaterial;
        if (MaterialLimit <= counts[index]) return; // 물약 종류별 제한
        if (0 >= counts[index]) return;

        totalCount++;
        if (plus)
            counts[index]++;
        else
            counts[index]--;

        ShowPotionColor();
    }

    // 조합 결과 표시
    private void ShowPotionColor()
    {
        var r = counts[(int)Material.R];
        var g = counts[(int)Material.G];
        var b = counts[(int)Material.B];

        var color = new Color(
            Math.Abs(r) * ColorRatio,
            Math.Abs(g) * ColorRatio,
            Math.Abs(b) * ColorRatio
        );
        potionImage.color = color;
        
        // rgb값이 음수일 경우 각 이펙트 출력
        ShowPotionEffect(r < 0, b < 0, g < 0);
    }

    // 재료 별 이펙트 출력
    private void ShowPotionEffect(bool redEffect, bool greenEffect, bool blueEffect)
    {
        // Todo : 색깔별 이펙트 표시
    }

    // 조합 결과 결정
    private void MakePotion()
    {
        var materialCount = new[] { counts[(int)Material.R], counts[(int)Material.G], counts[(int)Material.B] };

        ResetPotionMaterial();
        if (!DataManager.TryMakePotion(materialCount, out var potion))
        {
            // 없는 조합
            print("Food Not Found");
            return;
        }

        // story.GetPotionStory(potion);
        // Todo : 포션 레시피 해금 여부 결정, 제출 컨펌
    }
}