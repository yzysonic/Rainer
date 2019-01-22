using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using System;
using System.Linq;
using RainerLib;

public class JoyconManager: Singleton<JoyconManager>
{

    // Settings accessible via Unity
    public bool EnableIMU = true;
    public bool EnableLocalize = true;

	// Different operating systems either do or don't like the trailing zero
	private const ushort vendor_id = 0x57e;
	private const ushort vendor_id_ = 0x057e;
	private const ushort product_l = 0x2006;
	private const ushort product_r = 0x2007;

    public List<Joycon> j; // Array of all connected Joy-Cons

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);

        int i = 0;

		j = new List<Joycon>();
		bool isLeft = false;
		HIDapi.hid_init();

		IntPtr ptr = HIDapi.hid_enumerate(vendor_id, 0x0);
		IntPtr top_ptr = ptr;

		if (ptr == IntPtr.Zero)
		{
			ptr = HIDapi.hid_enumerate(vendor_id_, 0x0);
			if (ptr == IntPtr.Zero)
			{ 
				HIDapi.hid_free_enumeration(ptr);
				Debug.Log ("No Joy-Cons found!");
			}
		}
		hid_device_info enumerate;
        GameSetting.LoadAndSetData();
		while (ptr != IntPtr.Zero) {
			enumerate = (hid_device_info)Marshal.PtrToStructure (ptr, typeof(hid_device_info));

			Debug.Log (enumerate.product_id);
				if (enumerate.product_id == product_l || enumerate.product_id == product_r) {
					if (enumerate.product_id == product_l) {
						isLeft = true;
					    Debug.Log ("Left Joy-Con connected.");
                        Debug.Log($"SerialNumber: {enumerate.serial_number}");
                } else if (enumerate.product_id == product_r) {
						isLeft = false;
						Debug.Log ("Right Joy-Con connected.");
                        Debug.Log($"SerialNumber: {enumerate.serial_number}");
                } else {
						Debug.Log ("Non Joy-Con input device skipped.");
					}
					IntPtr handle = HIDapi.hid_open_path (enumerate.path);
					HIDapi.hid_set_nonblocking (handle, 1);
					j.Add (new Joycon (handle, EnableIMU, EnableLocalize & EnableIMU, 0.05f, isLeft, enumerate.serial_number));
					++i;
				}
				ptr = enumerate.next;
			}
		HIDapi.hid_free_enumeration (top_ptr);
    }

    void Start()
    {
		for (int i = 0; i < j.Count; ++i)
		{
			Debug.Log (i);
			Joycon jc = j [i];
			byte LEDs = 0x0;
			LEDs |= (byte)(0x1 << i);
			jc.Attach (leds_: LEDs);
			jc.Begin ();
		}
    }

    void Update()
    {
		for (int i = 0; i < j.Count; ++i)
		{
			j[i].Update();
		}
    }

    protected override void OnDestroy()
    {
        for (int i = 0; i < j.Count; ++i)
        {
            j[i].Detach();
        }
    }

    public static bool GetButton(Joycon.Button button)
    {
        return Instance.j.Any(j => j.GetButton(button));
    }
    public static bool GetButtonUp(Joycon.Button button)
    {
        return Instance.j.Any(j => j.GetButtonUp(button));
    }
    public static bool GetButtonDown(Joycon.Button button)
    {
        return Instance.j.Any(j => j.GetButtonDown(button));
    }

    public static float[] GetStick()
    {
        var sum = new float[2];
        sum[0] = Instance.j.Sum(j => j.GetStick()[0]);
        sum[1] = Instance.j.Sum(j => j.GetStick()[1]);
        return sum;
    }

}
