using UnityEngine;

namespace Core.Sounds
{
    public class SoundObject : MonoBehaviour
    {
        public bool Dynamic = true;

        public SphereCollider soundSphere;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void Play(AudioClip clip, Vector3 position, float volume = 1f, float maxDistance = 10f, float time = 0f, bool loop = false, bool detectable = true)
        {
            transform.position = position;

            Play(clip, volume, maxDistance, time, loop, detectable: detectable);
        }

        private void Play(AudioClip clip, float volume = 1f, float maxDistance = 10f, float time = 0f, bool loop = false, bool detectable = true)
        {
            _source.clip = clip;
            _source.volume = volume;
            _source.loop = loop;
            _source.time = time;
            _source.maxDistance = maxDistance;

            if (detectable)
                soundSphere.radius = maxDistance;
            else
                soundSphere.gameObject.SetActive(false);
            _source.Play();
        }

        public bool Playing()
        {
            return _source.isPlaying;
        }

        public void Stop()
        {
            _source.Stop();
        }

        private void Update()
        {
            if (Dynamic && !_source.isPlaying)
                Destroy(gameObject);
        }
    }
}