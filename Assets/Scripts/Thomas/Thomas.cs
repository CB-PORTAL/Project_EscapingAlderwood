using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuRound
{
    public class Thomas : MonoBehaviour
    {
        public AudioClip m_nearSound, m_farSound;
        private Mabel m_Mabel { get; set; }
        private AudioSource m_audioSource { get; set; }
        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
            m_Mabel = GameObject.FindWithTag("Mabel").GetComponent<Mabel>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            var distance = Vector2.Distance(m_Mabel.transform.position, transform.position);
            if (distance > m_audioSource.minDistance)
            {
                m_audioSource.clip = m_farSound;
                if(!m_audioSource.isPlaying)
                    m_audioSource.Play();
            }
            else if(m_audioSource.minDistance <= m_audioSource.minDistance) 
            {
                m_audioSource.clip = m_nearSound;
                if (!m_audioSource.isPlaying)
                    m_audioSource.Play();
            }
        }
    }
}
