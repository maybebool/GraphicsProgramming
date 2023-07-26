#version 330 core
uniform vec3 fColor;
uniform sampler2D baseColorMap;

smooth in vec2 fTexCoords;

out vec4 FragColor;

void main()
{
    vec3 baseColor = texture(baseColorMap, fTexCoords).rgb;
    FragColor = vec4(fColor * baseColor, 0.5f);
}