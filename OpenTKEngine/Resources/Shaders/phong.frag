#version 130

in vec2 texCoord0;

out vec4 fragColor;

uniform sampler2D texUnit0;

struct Material{
	vec4 diffuse;
	vec4 ambient;
	vec4 specular;
	vec4 emissive;
};

uniform Material material;

struct Light{
	vec3 position;
	vec3 direction;
	vec4 diffuse;
	vec4 ambient;
	vec4 specular;
};

uniform Light light;

varying vec3 normal;
varying vec3 vertex_to_light_vector;

void main(){
   vec3 normalized_normal = normalize(normal);
   vec3 normalized_vertex_to_light_vector = normalize(vertex_to_light_vector);
   float diffuseCoefficient = clamp(dot(normalized_normal, normalized_vertex_to_light_vector), 0.0, 1.0);
   
   vec4 colorFromLight = (material.ambient * light.ambient) + (material.diffuse * light.diffuse) * diffuseCoefficient;
   vec4 color = vec4(colorFromLight.rgb*texture2D(texUnit0, vec2(texCoord0.s, 1.0-texCoord0.t)).rgb, 1.0);
   fragColor = color;
}