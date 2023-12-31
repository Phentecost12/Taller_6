using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class Enemy_Spawn_System : MonoBehaviour
{
    [Header("Bachitombo")]
    [Space(5)]
    [SerializeField] private Bachitombo _bachitombo_Prefab;
    [SerializeField] private int _bachitombo_Pool_Default_Capacity;
    [SerializeField] private int _bachitombo_Pool_Max_Capacity;
    [SerializeField] private bool _bachitombo_Pool_Collection_Check;
    private ObjectPool<Bachitombo> _bachitombo_Pool;

    [Header("Tombo")]
    [Space(5)]
    [SerializeField] private Tombo _tombo_Prefab;
    [SerializeField] private int _tombo_Pool_Default_Capacity;
    [SerializeField] private int _tombo_Pool_Max_Capacity;
    [SerializeField] private bool _tombo_Pool_Collection_Check;
    private ObjectPool<Tombo> _tombo_Pool;

    [Header("Tombo Tactico")]
    [Space(5)]
    [SerializeField] private Tombo_Tactico _tombo_Tactico_Prefab;
    [SerializeField] private int _tombo_Tactico_Pool_Default_Capacity;
    [SerializeField] private int _tombo_Tactico_Pool_Max_Capacity;
    [SerializeField] private bool _tombo_Tactico_Pool_Collection_Check;
    private ObjectPool<Tombo_Tactico> _tombo_Tactico_Pool;

    [Header("Tombo Con Perro")]
    [Space(5)]
    [SerializeField] private Tombo_Con_Perro _tombo_Con_Perro_Prefab;
    [SerializeField] private int _tombo_Con_Pero_Pool_Default_Capacity;
    [SerializeField] private int _tombo_Con_Pero_Pool_Max_Capacity;
    [SerializeField] private bool _tombo_Con_Perro_Pool_Collection_Check;
    private ObjectPool<Tombo_Con_Perro> _tombo_Con_Pero_Pool;
    
    [Header("Esmad")]
    [Space(5)]
    [SerializeField] private Esmad _esmad_Prefab;
    [SerializeField] private int _esmad_Pool_Default_Capacity;
    [SerializeField] private int _esmad_Pool_Max_Capacity;
    [SerializeField] private bool _esmad_Pool_Collection_Check;
    private ObjectPool<Esmad> _esmad_Pool;
    private List<Enemy> _spawned_Enemy_Wave;
    private bool _active_Wave = false;
    public static Enemy_Spawn_System Instance {get; private set;} = null;
    public static event Action OnWaveEnd;
    private List<int> paths_Index = new List<int>();
    int count,index;

    private bool[,] Enemies_Found;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }
 
    void Start()
    {
        _bachitombo_Pool = new ObjectPool<Bachitombo>(()=> {
            return Instantiate(_bachitombo_Prefab);
        }, bachitombo => {
            bachitombo.gameObject.SetActive(true);
        }, bachitombo =>{
            bachitombo.gameObject.SetActive(false);
        }, bachitombo => {
            Destroy(bachitombo.gameObject);
        },_bachitombo_Pool_Collection_Check, _bachitombo_Pool_Default_Capacity,_bachitombo_Pool_Max_Capacity);

        _tombo_Pool = new ObjectPool<Tombo>(() => {
            return Instantiate(_tombo_Prefab);
        }, tombo => {
            tombo.gameObject.SetActive(true);
        }, tombo => {
            tombo.gameObject.SetActive(false);
        }, tombo => {
            Destroy(tombo.gameObject);
        }, _tombo_Pool_Collection_Check, _tombo_Pool_Default_Capacity,_tombo_Pool_Max_Capacity);

        _tombo_Tactico_Pool = new ObjectPool<Tombo_Tactico>(()=> {
            return Instantiate(_tombo_Tactico_Prefab);
        }, tombo_Tactico => {
            tombo_Tactico.gameObject.SetActive(true);
        }, tombo_Tactico => {
            tombo_Tactico.gameObject.SetActive(false);
        }, tombo_Tactico => {
            Destroy(tombo_Tactico.gameObject);
        }, _tombo_Tactico_Pool_Collection_Check,_tombo_Tactico_Pool_Default_Capacity,_tombo_Tactico_Pool_Max_Capacity);

        _tombo_Con_Pero_Pool = new ObjectPool<Tombo_Con_Perro>(()=> {
            return Instantiate(_tombo_Con_Perro_Prefab);
        }, tombo_Con_perro => {
            tombo_Con_perro.gameObject.SetActive(true);
        }, tombo_Con_perro =>{
            tombo_Con_perro.gameObject.SetActive(false);
        }, tombo_Con_Perro => {
            Destroy(tombo_Con_Perro.gameObject);
        }, _tombo_Con_Perro_Pool_Collection_Check, _tombo_Con_Pero_Pool_Default_Capacity, _tombo_Con_Pero_Pool_Max_Capacity);

        _esmad_Pool = new ObjectPool<Esmad>(() =>{
            return Instantiate(_esmad_Prefab);
        }, esmad => {
            esmad.gameObject.SetActive(true);
        }, esmad => {
            esmad.gameObject.SetActive(false);
        }, esmad=> {
            Destroy(esmad.gameObject);
        },_esmad_Pool_Collection_Check, _esmad_Pool_Default_Capacity, _esmad_Pool_Max_Capacity);
        
        _spawned_Enemy_Wave = new List<Enemy>();
        Enemies_Found = new bool[2,5];
    }

    void Update()
    {
        if(!_active_Wave) return;

        if(_spawned_Enemy_Wave.Count <= 0)
        {
            Debug.Log("Ronda Terminada");
            OnWaveEnd();
            _active_Wave = false;
        }
    }

    void Fill_Pools()
    {
        for (int i = 0; i<_bachitombo_Pool_Default_Capacity;i++)
        {
            var enemy = _bachitombo_Pool.Get();
            _bachitombo_Pool.Release(enemy);
        }

        for (int j = 0; j< _tombo_Pool_Default_Capacity;j++)
        {
            var enemy = _tombo_Pool.Get();
            _tombo_Pool.Release(enemy);
        }

        for (int k = 0; k< _tombo_Tactico_Pool_Default_Capacity;k++)
        {
            var enemy = _tombo_Tactico_Pool.Get();
            _tombo_Tactico_Pool.Release(enemy);
        }

        for (int l = 0; l< _tombo_Con_Pero_Pool_Default_Capacity;l++)
        {
            var enemy = _tombo_Con_Pero_Pool.Get();
            _tombo_Con_Pero_Pool.Release(enemy);
        }

        for (int m = 0; m< _esmad_Pool_Default_Capacity;m++)
        {
            var enemy = _esmad_Pool.Get();
            _esmad_Pool.Release(enemy);
        }
    }

    public IEnumerator SpawnWave (Wave wave, bool tutorial)
    {
        index = 0;
        count = 0;
        int enemies_per_paths; 
        (enemies_per_paths,paths_Index) = wave.GetPahts();

        

        if(wave._tombo_Count >0)
        {
            
            if(!Enemies_Found[0,1])
            {
                Enemies_Found[0,1] = true;
            }

            for (int i = 0; i< wave._tombo_Count;i++)
            {
                var enemy = _tombo_Pool.Get();
                _spawned_Enemy_Wave.Add(enemy);
                enemy.Config(OnDeath,OnReach, paths_Index[index]);
                count++;
                if(count == enemies_per_paths && index < paths_Index.Count-1) {index++; count = 0;}
                yield return new WaitForSeconds(0.2f);
            }
        }

        if(wave._bachitombo_Count >0)
        {
            if(!Enemies_Found[0,0])
            {
                Enemies_Found[0,0] = true;
            }

            for (int i = 0; i< wave._bachitombo_Count;i++)
            {
                var enemy = _bachitombo_Pool.Get();
                _spawned_Enemy_Wave.Add(enemy);
                enemy.Config(OnDeath,OnReach,paths_Index[index]);
                count++;
                if(count == enemies_per_paths && index < paths_Index.Count-1) {index++; count = 0;}
                yield return new WaitForSeconds(0.2f);
            }
        }

        if(wave._tombo_Tactico_Count >0)
        {
            if(!Enemies_Found[0,2])
            {
                Enemies_Found[0,2] = true;
            }
            for (int i = 0; i< wave._tombo_Tactico_Count;i++)
            {
                var enemy = _tombo_Tactico_Pool.Get();
                _spawned_Enemy_Wave.Add(enemy);
                enemy.Config(OnDeath,OnReach, paths_Index[index]);

                count++;
                if(count == enemies_per_paths && index < paths_Index.Count-1) {index++; count = 0;}
                yield return new WaitForSeconds(0.2f);
            }
        }

        if(wave._tombo_Con_Perro_Count >0)
        {
            if(!Enemies_Found[0,3])
            {
                Enemies_Found[0,3] = true;
            }

            for (int i = 0; i< wave._tombo_Con_Perro_Count;i++)
            {
                var enemy = _tombo_Con_Pero_Pool.Get();
                _spawned_Enemy_Wave.Add(enemy);
                enemy.Config(OnDeath,OnReach,paths_Index[index]);
                count++;
                if(count == enemies_per_paths && index < paths_Index.Count-1) {index++; count = 0;}
                yield return new WaitForSeconds(0.2f);
            }
        }

        if(wave._esmad_Count >0)
        {
            if(!Enemies_Found[0,4])
            {
                Enemies_Found[0,4] = true;
            }

            for (int i = 0; i< wave._esmad_Count;i++)
            {
                var enemy = _esmad_Pool.Get();
                _spawned_Enemy_Wave.Add(enemy);
                enemy.Config(OnDeath,OnReach,paths_Index[index]);
                count++;
                if(count == enemies_per_paths && index < paths_Index.Count-1) {index++; count = 0;}
                yield return new WaitForSeconds(0.2f);
            }
        }

        _active_Wave = true;
        if(!tutorial)StartCoroutine(Check_for_New_Enemy());
    }

    IEnumerator Check_for_New_Enemy()
    {
        for(int i = 0; i < Enemies_Found.GetLength(1); i++)
        {
            if(Enemies_Found[0,i] && !Enemies_Found[1,i])
            {
                
                foreach(Enemy en in _spawned_Enemy_Wave)
                {
                    en.stop();
                }
                UI_Manager.Instance.Enemies_Panels_Activations(i);
                yield return StartCoroutine(Wait_for_Input());
                UI_Manager.Instance.Enemies_Panels_Activations(i);
                foreach(Enemy en in _spawned_Enemy_Wave)
                {
                    en.stop();
                }
                Enemies_Found[1,i] = true;
                break;
            }
        }
    }

    IEnumerator Wait_for_Input()
    {
        yield return null;
        while(Input.touchCount==0 && !Input.GetMouseButton(0))
        {
            yield return null;
        }
    }

    private void OnDeath(Enemy enemy, int i)
    {
        Player_Interaction.Instance.GetCoins(i);
        RemoveEnemy(enemy);
    }

    private void OnReach(Enemy enemy, int i)
    {
        Health.Instance.TakeDamage(i);
        RemoveEnemy(enemy);
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _spawned_Enemy_Wave.Remove(enemy);
        if(enemy is Bachitombo){_bachitombo_Pool.Release((Bachitombo)enemy);}
        else if (enemy is Tombo){_tombo_Pool.Release((Tombo)enemy);}
        else if (enemy is Tombo_Tactico){_tombo_Tactico_Pool.Release((Tombo_Tactico)enemy);}
        else if (enemy is Tombo_Con_Perro){_tombo_Con_Pero_Pool.Release((Tombo_Con_Perro)enemy);}
        else if (enemy is Esmad){_esmad_Pool.Release((Esmad)enemy);}
    }
}

[System.Serializable]
public class Wave
{
   public int _bachitombo_Count;
   public int _tombo_Count;
   public int _tombo_Tactico_Count;
   public int _tombo_Con_Perro_Count;
   public int _esmad_Count;
   public bool Path_01;
   public bool Path_02;
   public bool Path_03;
   public bool Path_04;

    public (int,List<int>) GetPahts()
    {
        int totalCount = _bachitombo_Count+ _tombo_Count +_tombo_Tactico_Count + _tombo_Con_Perro_Count + _esmad_Count;
        List<int> paths = new List<int>();
        int number_of_Paths = 0;

        if(Path_01)
        {
            number_of_Paths ++;
            paths.Add(0);
        }

        if(Path_02)
        {
            number_of_Paths ++;
            paths.Add(1);
        }

        if(Path_03)
        {
            number_of_Paths ++;
            paths.Add(2);
        }

        if(Path_04)
        {
            number_of_Paths ++;
            paths.Add(3);
        }

        int enemy_per_Path = (int)MathF.Round(totalCount/number_of_Paths);

        return(enemy_per_Path,paths);

    }

}
